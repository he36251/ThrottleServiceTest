namespace ThrottleServiceTest
{
    public static class ThrottleServiceConstant
    {
        public static int ThrottleThreshold = 2;
        public static double DelayMillisec = 20000;
    }

    public enum QueueItemTypes
    {
        Comment,
        Idea
    }
}