using System.Collections.Generic;
using System.Windows;
using AdLib.Common.Interfaces;
using AdLib.Common.Utilities;

namespace AdLib.UI.Dialogs
{
    public partial class WindowSelectionDialog : Window, IWindowDialog
    {
        public WindowInfo SelectedWindow { get; private set; }

        public WindowSelectionDialog(List<WindowInfo> windows)
        {
            InitializeComponent();
            WindowListBox.ItemsSource = windows;
        }

        public WindowInfo ShowDialog(List<WindowInfo> windows)
        {
            // Optionally create a new dialog instance if needed
            var dialog = new WindowSelectionDialog(windows);
            return dialog.ShowDialog() == true ? dialog.SelectedWindow : null;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowListBox.SelectedItem is WindowInfo selectedWindow)
            {
                SelectedWindow = selectedWindow;
                DialogResult = true;
            }
        }
    }
}
