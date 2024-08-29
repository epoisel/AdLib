using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using AdLib.UI;
using AdLib.UI.Services;
using AdLib.Common.Interfaces;
using AdLib.Automation.Actions;
using AdLib.Automation.Interfaces;
using AdLib.Common.Services;

namespace AdLib
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = _serviceProvider.GetService<ILogger<App>>();
            if (logger != null)
            {
                logger.LogInformation("Service provider initialized successfully.");
            }
            else
            {
                Console.WriteLine("Logger not initialized. Check service provider setup.");
            }

        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Registering services
            services.AddSingleton<IWindowSelectionService, WindowSelectionService>();
            services.AddTransient<IButtonClickService, ButtonClickService>();
            services.AddTransient<IProcessService, ProcessService>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            services.AddLogging(configure => configure.AddConsole().AddDebug()); // Ensure logging setup

            // Registering automation actions
            services.AddTransient<ClickButtonAction>();
            services.AddTransient<FocusApplicationAction>();
            services.AddTransient<InputTextAction>();
            services.AddTransient<OpenApplicationAction>();
            services.AddTransient<SubmitAction>();
            services.AddTransient<CloseApplicationAction>();

            // Register the AutomationBuilder window
            services.AddSingleton<AutomationBuilder>(provider =>
            {
                var windowSelectionService = provider.GetRequiredService<IWindowSelectionService>();
                var logger = provider.GetRequiredService<ILogger<AutomationBuilder>>();
                return new AutomationBuilder(provider, windowSelectionService, logger);
            });
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var automationBuilder = AutomationBuilder.WindowFactory.CreateAutomationBuilder(_serviceProvider);
            var logger = _serviceProvider.GetRequiredService<ILogger<AutomationBuilder>>();
            logger.LogInformation("AutomationBuilder window is being shown.");
            automationBuilder.Show();
        }

    }
}
