using System.Collections.Generic;
using System.Windows;
using AdLib.Common.Utilities; // Assuming you have moved WindowInfo to AdLib.Common

namespace AdLib.UI
{
    public partial class WindowSelectionDialog : Window
    {
        public WindowInfo SelectedWindow { get; private set; }

        public WindowSelectionDialog(List<WindowInfo> windows)
        {
            InitializeComponent(); // Ensures that XAML components are initialized
            WindowListBox.ItemsSource = windows; // Ensures ListBox is populated with windows
        }

        // Ensure this method exists and is public
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
