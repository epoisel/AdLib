using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using AdLib.UI.ViewModels;
using AdLib.UI.Services;
using AdLib.Common.Services;

namespace AdLib.UI
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register ViewModels and Main Window
            services.AddSingleton<AutomationBuilderViewModel>();
            services.AddSingleton<AutomationBuilder>();

            // Register ActionManager - keeping this as it might still be needed even without actions
            services.AddSingleton<IActionManager, ActionManager>();

            // For now, we are not registering specific IAutomationAction implementations
            // This can be reintroduced once individual actions are ready for refactoring

            // Configure logging
            services.AddLogging(configure => configure.AddConsole().AddDebug());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Console.WriteLine("OnStartup called"); // Debug line

            try
            {
                var mainWindow = _serviceProvider.GetRequiredService<AutomationBuilder>();
                mainWindow.Show();
                Console.WriteLine("Main window shown successfully"); // Debug line
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error showing main window: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(); // Shut down the application if the main window can't be shown
            }
        }
    }
}
