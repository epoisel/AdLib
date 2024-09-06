using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace AdLib.Contracts.ViewModels
{
    public class ActionPropertyViewModel : INotifyPropertyChanged
    {
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }

        private object _propertyValue;
        public object PropertyValue
        {
            get => _propertyValue;
            set
            {
                _propertyValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PropertyValue)));
                SetProperty?.Invoke(value); // Call action's property setter if available
            }
        }

        // Setter function provided by the action itself
        public Action<object> SetProperty { get; set; }

        // Optional command to invoke an action (e.g., file browsing)
        public Action BrowseAction { get; set; }

        public ICommand BrowseCommand => new RelayCommand(() =>
        {
            if (BrowseAction != null)
            {
                Debug.WriteLine("BrowseCommand invoked, BrowseAction is not null.");
                BrowseAction.Invoke();
            }
            else
            {
                Debug.WriteLine("BrowseCommand invoked, but BrowseAction is null.");
            }
        });

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
