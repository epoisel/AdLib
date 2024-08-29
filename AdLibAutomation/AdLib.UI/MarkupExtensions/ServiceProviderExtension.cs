// ServiceProviderExtension.cs
using AdLib.UI.Services;
using System;
using System.Windows.Markup;

namespace AdLib.UI.MarkupExtensions
{
    public class ServiceProviderExtension : MarkupExtension
    {
        public Type ServiceType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ServiceType == null)
                throw new InvalidOperationException("ServiceType is not set.");

            // Use GetService(Type) instead of a generic method
            return ServiceLocator.ServiceProvider.GetService(ServiceType);
        }
    }
}
