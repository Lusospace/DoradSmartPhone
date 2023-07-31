namespace DoradSmartphone.Helpers
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            services[typeof(T)] = service;
        }

        public static T Get<T>()
        {
            if (!services.TryGetValue(typeof(T), out object service))
            {
                throw new InvalidOperationException($"Service {typeof(T).FullName} not registered.");
            }

            return (T)service;
        }

        public static bool IsRegistered<T>()
        {
            return services.ContainsKey(typeof(T));
        }
    }
}
