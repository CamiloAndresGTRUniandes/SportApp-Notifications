namespace SportAppNotifications.Application.Constants ;

    public static class QueuesSportApp
    {
        public const string UserUpdateQueue = "sportapp.users.userupdate";
        public const string UserRecommendationsQueue = "sportapp.users.recomendaciones";
        public static List<string> Queues { get; } = new() { "sportapp.users.events", "sportapp.users.recomendaciones" };
    }
