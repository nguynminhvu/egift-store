using EGiftStore.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.Interface;
using System.Text;

namespace EGiftStore.MiddlewareInvoke.Invoke
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeSpanSecond;

        public CacheAttribute(int timeSpanSecond = 20000)
        {
            _timeSpanSecond = timeSpanSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();
            if (!cacheConfiguration.Enable)
            {
                await next();
                return;
            }

            var cacheKey = GetCacheKeyFromRequest(context.HttpContext.Request);
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheResponse = await cacheService.GetCacheAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult oor)
            {
                await cacheService.SetCacheAsync(cacheKey, oor?.Value!, TimeSpan.FromSeconds(_timeSpanSecond));
            }
        }

        private static string GetCacheKeyFromRequest(HttpRequest request)
        {
            var cacheKey = new StringBuilder();
            cacheKey.Append($"{request.Path}");
            cacheKey.Append("?");

            foreach (var (param, value) in request.Query.OrderBy(x => x.Key))
            {
                cacheKey.Append($"{param}={value}/");
            }
            return cacheKey.ToString();
        }
    }
}
