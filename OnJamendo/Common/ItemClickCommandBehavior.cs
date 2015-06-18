using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace OnJamendo.Common
{
    public static class ItemClickCommandBehavior
    {
        #region Command Attached Property
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ItemClickCommandBehavior), 
            new PropertyMetadata(null, OnCommandChanged));
        #endregion

        #region Behavior implementation
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var lvb = d as ListViewBase;
            if (lvb != null)
                lvb.ItemClick += OnClick;

            var flp = d as FlipView;
            if (flp != null)
                flp.PointerPressed += OnPointerPressed;
        }

        private static void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var flp = sender as Selector;
            if (flp == null || flp.SelectedItem == null) return;
            var cmd = flp.GetValue(CommandProperty) as ICommand;
            
            if (cmd != null && cmd.CanExecute(flp.SelectedItem))
                cmd.Execute(flp.SelectedItem);
        }

        private static void OnClick(object sender, ItemClickEventArgs e)
        {
            var lvb = sender as ListViewBase;
            if (lvb == null) return;
            var cmd = lvb.GetValue(CommandProperty) as ICommand;
            
            if (cmd != null && cmd.CanExecute(e.ClickedItem))
                cmd.Execute(e.ClickedItem);
        }
        #endregion
    }
}
