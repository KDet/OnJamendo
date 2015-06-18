namespace OnJamendo.Service
{
    public interface INavigationService
    {
        void GoHome();
        void GoBack();
        void GoForward();
        void Navigate(string pageName);
        void Navigate(string pageName, object parameter);
        bool CanGoBack { get; }
        bool CanGoForward { get; }
    }
}
