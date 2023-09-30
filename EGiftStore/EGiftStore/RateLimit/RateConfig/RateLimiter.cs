
using EGiftStore.RateLimit.Model;
using Microsoft.AspNetCore.RateLimiting;

namespace EGiftStore.RateLimit.RateConfig
{
    public static class RateLimiter
    {
        public static void UseRateLimit(this IServiceCollection services)
        {
            services.AddRateLimiter(x => x.AddSlidingWindowLimiter(policyName: RateLimitOption.MyLimiter, options =>
            {
                options.QueueLimit = RateLimitOption.QueueLimit;
                options.PermitLimit = RateLimitOption.PermitLimit;
                options.Window = TimeSpan.FromSeconds(RateLimitOption.Window);
                options.SegmentsPerWindow = RateLimitOption.SegmentPerWindow;
                options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            }));
        }
    }
}
