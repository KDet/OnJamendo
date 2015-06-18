using System;
using System.Collections.Generic;
using System.Net.Http;
using OnJamendo.Common;
using OnJamendo.Service;
using OnJamendo.ViewModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OnJamendo.View
{
    public class BaseView : LayoutAwarePage
    {
        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            var viewModel = DataContext as BaseViewModel;
            if (viewModel != null)
                viewModel.LoadState(navigationParameter, pageState);
        }

        protected override void SaveState(Dictionary<string, object> pageState)
        {
            var viewModel = DataContext as BaseViewModel;
            if (viewModel != null) 
                viewModel.SaveState(pageState);
        }

        //Здерто
         #region DataContextChanged handling 
        // Workaround for no DataContextChanged event in WinRT
        // Set a binding for this dependency property to an empty binding and hook up a change callback handler
        // The change callback handler becomes the equivalent of a DataContextChanged event since the property will be set each time the DataContext changes
        public static readonly DependencyProperty DataContextChangedWatcherProperty =
            DependencyProperty.Register("DataContextChangedWatcher", typeof(object), typeof(BaseView), 
            new PropertyMetadata(null, OnDataContextChanged));

        public object DataContextChangedWatcher
        {
            get { return GetValue(DataContextChangedWatcherProperty); }
            set { SetValue(DataContextChangedWatcherProperty, value); }
        }

        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = ((BaseView)d).DataContext as BaseViewModel;
            if (vm != null) vm.NavigationService = NavigationService.Current;
        }

        public BaseView()
        {
            BindingOperations.SetBinding(this, DataContextChangedWatcherProperty, new Binding());
        }

        #endregion
    }
}
