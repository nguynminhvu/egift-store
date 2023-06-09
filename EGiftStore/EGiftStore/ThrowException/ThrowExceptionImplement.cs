using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EGiftStore.ThrowException
{
    public class ThrowExceptionImplement : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case Exception ex: context.Result = new BadRequestObjectResult(ex.Message); break;
            }
        }
    }
}
