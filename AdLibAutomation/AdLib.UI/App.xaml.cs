using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using AdLib.UI.ViewModels;
using AdLib.Common.Services;
using AdLib.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;


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

            // Check if serviceProvider is successfully created
            Debug.WriteLine("ServiceProvider initialized.");
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register ViewModels and Main Window
            services.AddSingleton<AutomationBuilderViewModel>(); // ViewModel
            services.AddSingleton<AutomationBuilder>();          // Main window

            // Plugin directory for loading actions
            string pluginDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\AdLib.engine\Plugins\"));
            Debug.WriteLine($"Resolved plugin directory: {pluginDirectory}");


            // Register ActionManager, passing the plugin directory
            services.AddSingleton<IActionManager>(provider =>
                new ActionManager(pluginDirectory));

            // Register logging
            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddDebug();
            });

            Debug.WriteLine("Services configured.");
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        protected override void OnStartup(StartupEventArgs e)
        {
            AllocConsole();  // Optional: Open the console for logging

            base.OnStartup(e);

            try
            {
                // Resolve and show the main window
                var mainWindow = _serviceProvider.GetRequiredService<AutomationBuilder>();
                mainWindow.Show();

                Console.WriteLine("Main window shown.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error showing main window: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}
