using Windows.UI.Xaml.Controls;

// Шаблон элемента страницы сгруппированных элементов задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234231

namespace OnJamendo.View
{
    /// <summary>
    /// Страница, на которой отображается сгруппированная коллекция элементов.
    /// </summary>
    public sealed partial class PlayerPage : BaseView
    {
        public PlayerPage()
        {
            InitializeComponent();
        }

        //TODO зробити команду а не подію (полегшить unitTest)
        private void PlayListlistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = sender as ListViewBase;
            if (lv != null)
                lv.ScrollIntoView(lv.SelectedItem, ScrollIntoViewAlignment.Default);
        }
    }
}
