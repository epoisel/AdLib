using System.Windows;
using System.Windows.Input;

namespace AdLib.UI.Behaviors
{
    public static class DragDropBehavior
    {
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.RegisterAttached(
                "DropCommand",
                typeof(ICommand),
                typeof(DragDropBehavior),
                new PropertyMetadata(null, OnDropCommandChanged));

        public static ICommand GetDropCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DropCommandProperty);
        }

        public static void SetDropCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DropCommandProperty, value);
        }

        private static void OnDropCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uiElement)
            {
                if (e.NewValue != null)
                {
                    uiElement.Drop += OnDrop;
                }
                else
                {
                    uiElement.Drop -= OnDrop;
                }
            }
        }

        private static void OnDrop(object sender, DragEventArgs e)
        {
            var uiElement = sender as UIElement;
            var command = GetDropCommand(uiElement);
            if (command != null && command.CanExecute(e))
            {
                command.Execute(e);
            }
        }

        public static readonly DependencyProperty DragOverCommandProperty =
            DependencyProperty.RegisterAttached(
                "DragOverCommand",
                typeof(ICommand),
                typeof(DragDropBehavior),
                new PropertyMetadata(null, OnDragOverCommandChanged));

        public static ICommand GetDragOverCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DragOverCommandProperty);
        }

        public static void SetDragOverCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DragOverCommandProperty, value);
        }

        private static void OnDragOverCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uiElement)
            {
                if (e.NewValue != null)
                {
                    uiElement.DragOver += OnDragOver;
                }
                else
                {
                    uiElement.DragOver -= OnDragOver;
                }
            }
        }

        private static void OnDragOver(object sender, DragEventArgs e)
        {
            var uiElement = sender as UIElement;
            var command = GetDragOverCommand(uiElement);
            if (command != null && command.CanExecute(e))
            {
                command.Execute(e);
            }
        }
    }
}
