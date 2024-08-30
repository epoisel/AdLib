using System.Collections.Generic;
using System.Windows;
using AdLib.Common.Interfaces;
using AdLib.Common.Utilities;

namespace AdLib.UI.Dialogs
{
    public partial class WindowSelectionDialog : Window, IWindowDialog
    {
        public WindowInfo SelectedWindow { get; private set; }

        // Single constructor definition
        public WindowSelectionDialog(List<WindowInfo> windows)
        {
            InitializeComponent();
            WindowListBox.ItemsSource = windows; // Populate the ListBox with window info
        }

        // Implementing the IWindowDialog interface
        public WindowInfo ShowDialog(List<WindowInfo> windows)
        {
            // Reuse existing dialog or create new instance as needed
            var dialog = new WindowSelectionDialog(windows);
            if (dialog.ShowDialog() == true)
            {
                return dialog.SelectedWindow;
            }
            return null; // No selection made or dialog was canceled
        }

        // Single OkButton_Click method
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowListBox.SelectedItem is WindowInfo selectedWindow)
            {
                SelectedWindow = selectedWindow;
                DialogResult = true; // Close the dialog and signal success
            }
        }
    }
}
