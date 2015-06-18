using System;
using Windows.UI.Xaml.Controls;

namespace OnJamendo.Service
{
    public class NavigationService :INavigationService
    {
        private static NavigationService _instance;

        public static INavigationService Current
        {
            get { return _instance ?? (_instance = new NavigationService()); }
        }

        public static Frame Frame { get; set; }

        public static string ViewNamespace { get; set; }

        public void GoHome()
        {
            if (Frame == null) return;
            while (Frame.CanGoBack) Frame.GoBack();
        }

        public void GoBack()
        {
            if (Frame != null && Frame.CanGoBack) Frame.GoBack();
        }

        public void GoForward()
        {
            if (Frame != null && Frame.CanGoForward) Frame.GoForward();
        }

        public void Navigate(string pageName)
        {
            Navigate(pageName, null);
        }

        public void Navigate(string pageName, object parameter)
        {
            var viewTypeName = string.Format("{0}.{1}", string.IsNullOrEmpty(ViewNamespace) ? typeof(View.BaseView).Namespace : ViewNamespace, pageName);
            var viewType = Type.GetType(viewTypeName);

            Frame.Navigate(viewType, parameter);
        }

        public bool CanGoBack
        {
            get { return Frame.CanGoBack; }
        }

        public bool CanGoForward
        {
            get { return Frame.CanGoForward; }
        }
    }
}
