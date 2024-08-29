using System;
using Microsoft.Extensions.DependencyInjection;

namespace AdLib.UI.Services
{
    public static class ServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static T GetService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }
}
