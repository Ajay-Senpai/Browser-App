using Microsoft.Extensions.DependencyInjection;
using System;

namespace BrowserApp.Services
{
    public static class AppServices
    {
        private static IServiceProvider _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider), "Service provider cannot be null.");
            }
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>()
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider has not been initialized.");
            }
            return _serviceProvider.GetService<T>();
        }
    }
}
