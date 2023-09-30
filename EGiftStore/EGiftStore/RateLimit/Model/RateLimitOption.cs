namespace EGiftStore.RateLimit.Model
{
    public static class RateLimitOption
    {
        public const string MyLimiter = "EgiftLimiter";
        public static int Window { get; set; } = 1;
        public static int PermitLimit { get; set; } = 5;
        public static int QueueLimit { get; set; } = 2;
        public static int SegmentPerWindow { get; set; } = 1;
    }
}
