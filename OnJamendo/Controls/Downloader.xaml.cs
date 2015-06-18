using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OnJamendo.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnJamendo.Controls
{
    public sealed partial class Downloader : UserControl
    {
        public Downloader()
        {
            this.InitializeComponent();
        }

        public IList<Track> DownloadList
        {
            get { return (IList<Track>)GetValue(PlayListSourceProperty); }
            set { SetValue(PlayListSourceProperty, value); }
        }

        public DependencyProperty PlayListSourceProperty =
            DependencyProperty.Register("DownloadList",
                                        typeof(IList<Track>),
                                        typeof(Player),
                                        new PropertyMetadata(null, PlayListSourceChangedCallback));

        private static async void PlayListSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await Window.Current.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var player = (Downloader)d;
                var newSource = e.NewValue as IList<Track>;
                if (newSource == null) return;
                player.DownloadListView.ItemsSource = newSource;
            });
        }
    }
}
