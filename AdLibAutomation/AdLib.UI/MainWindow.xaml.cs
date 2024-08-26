using System;
using System.IO;
using System.Windows;
using AdLib.Engine.Services;

namespace AdLib.UI
{
    public partial class MainWindow : Window
    {
        private AdLibPluginLoader _pluginLoader;

        public MainWindow()
        {
            InitializeComponent();
            _pluginLoader = new AdLibPluginLoader(@"C:\AdLibPlugins");
        }

        private void LoadPlugins_Click(object sender, RoutedEventArgs e)
        {
            var plugins = _pluginLoader.LoadPlugins();
            OutputTextBox.Text += "Plugins Loaded:\n";
            foreach (var plugin in plugins)
            {
                OutputTextBox.Text += $"{plugin.Name}\n";
            }
        }

        private void ExecutePlugins_Click(object sender, RoutedEventArgs e)
        {
            _pluginLoader.ExecuteAllPlugins();
            OutputTextBox.Text += "Plugins Executed\n";
        }
    }
}
