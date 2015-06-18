using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnJamendo.Common;
using OnJamendo.Repository;
using OnJamendo.Service;
using Windows.UI.Popups;

namespace OnJamendo.ViewModel
{
    public class BaseViewModel : BindableBase
    {
        public virtual void LoadState(object navigationParameter, Dictionary<string, object> pageState) { }
        public virtual void SaveState(Dictionary<string, object> pageState) { }

        public INavigationService NavigationService { get; set; }
        public virtual bool CanGoBack
        {
            get {
                return NavigationService != null && NavigationService.CanGoBack;
            }
        }
        public virtual bool CanGoForward
        {
            get {
                return NavigationService != null && NavigationService.CanGoForward;
            }
        }

        protected readonly IJamendoRepository Repository = new JamendoRepository();

       
        protected virtual void Notify(string message, string header)
        {
            var messageDialog = new MessageDialog(string.Empty) {Content = message};
            if(!string.IsNullOrEmpty(header)) messageDialog.Title = header;
           // messageDialog.ShowAsync();
        }

        public BaseViewModel()
        {
            Repository.ConnectionLost += RepositoryOnConnectionLost; 
            Repository.ConnectionFound += RepositoryOnConnectionFound;
            RefreshPage = new RelayCommand(OnRefreshPage);
        }

        public RelayCommand RefreshPage { private set; get; }

        private void OnRefreshPage()
        {
            LoadState(null,null);
        }

        public virtual  void RepositoryOnConnectionFound(object sender, EventArgs eventArgs)
        {
            LoadState(null, null);
        }

        public virtual void RepositoryOnConnectionLost(object sender, EventArgs eventArgs)
        {
            Notify("Немає з'єднання з інтернетом","Error");
        }
    }
}
