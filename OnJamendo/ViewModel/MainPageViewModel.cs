using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using OnJamendo.Common;
using OnJamendo.Model;
using OnJamendo.Repository;
using Windows.UI.Popups;

namespace OnJamendo.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        //public MainPageViewModel()
        //{
        //    var pl = new PlayList
        //        {
        //            Duration = 100,
        //            Id = 2556,
        //            Name = "Play list",
        //            TrackCount = 10,
        //            Url = "url",
        //            Image = "https://imgjam.com/albums/s121/121766/covers/1.500.jpg"
        //        };
        //    var pls = new PlayList[2];
        //    pls[0] = pl;
        //    pls[1] = pl;

        //    TopPlayLists = new ObservableCollection<PlayList>(pls);


        //    var album = new Album()
        //        {
        //            Artist_Name = "ArtistName",
        //            Duration = 100502,
        //            Genre = "genre",
        //            Id = 12,
        //            Image = "https://imgjam.com/albums/s121/121766/covers/1.200.jpg",
        //            Name = "Name",
        //            TrackCount = 1,
        //            Url = "url"
        //        };
        //    var albms = new Album[4];
        //    albms[0] = albms[1] = albms[2] = albms[3] = album;
        //    TopAlbums = new ObservableCollection<Album>(albms);


        //    var track = new Track()
        //        {
        //            TrackCount = 1,
        //            Album_Image = "https://imgjam.com/albums/s121/121766/covers/1.200.jpg",
        //            Album_Name = "same album name",
        //            Duration = 63,
        //            Artist_Name = "artist 2",
        //            Id = 456,
        //            Name = "name"
        //        };
        //    var tracks = new Track[4];
        //    tracks[0] = tracks[1] = tracks[2] = tracks[3] = track;
        //    TopTracks = new ObservableCollection<Track>(tracks);
        //}

        #region ObservableCollections

        private ObservableCollection<Track> _tracks;

        public ObservableCollection<Track> TopTracks
        {
            get { return _tracks; }
            set { SetProperty(ref _tracks, value); }
        }

        private ObservableCollection<Album> _albums;

        public ObservableCollection<Album> TopAlbums
        {
            get { return _albums; }
            set { SetProperty(ref _albums, value); }
        }

        private ObservableCollection<PlayList> _playLists;

        public ObservableCollection<PlayList> TopPlayLists
        {
            get { return _playLists; }
            set { SetProperty(ref _playLists, value); }
        }

        #endregion

        public MainPageViewModel()
        {
            TrackSelectedCommand = new RelayCommand<MusicItem>(OnTitleSelected);
        }

       
        public override async void SaveState(Dictionary<string, object> pageState)
        {
            if (TopPlayLists != null)
                pageState["TopPlayLists"] = await JsonConvert.SerializeObjectAsync(TopPlayLists);

            if (TopTracks != null)
                pageState["TopTracks"] = await JsonConvert.SerializeObjectAsync(TopTracks);

            if (TopAlbums != null)
                pageState["TopAlbums"] = await JsonConvert.SerializeObjectAsync(TopAlbums);
        }

        public override async void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
           // if (string.IsNullOrEmpty(navigationParameter as string)) return;
            try
            {
                TopPlayLists = (pageState != null && pageState.ContainsKey("TopPlayLists"))
                                   ? await
                                     JsonConvert.DeserializeObjectAsync<ObservableCollection<PlayList>>(
                                         pageState["TopPlayLists"] as string)
                                   : await Repository.GetTopPlayLists(5);

                TopTracks = (pageState != null && pageState.ContainsKey("TopTracks"))
                                ? await
                                  JsonConvert.DeserializeObjectAsync<ObservableCollection<Track>>(
                                      pageState["TopTracks"] as string)
                                : await Repository.GetTopTracks(10);

                TopAlbums = (pageState != null && pageState.ContainsKey("TopAlbums"))
                                ? await
                                  JsonConvert.DeserializeObjectAsync<ObservableCollection<Album>>(
                                      pageState["TopAlbums"] as string)
                                : await Repository.GetTopAlbums(10);
            }
            catch (HttpRequestException)
            {
                new MessageDialog("Помилка підключення до інтернету", "Error").ShowAsync();
                //   Notify("Помилка підключення до інтернету", "Error");
            }
        }

        
        public RelayCommand<MusicItem> TrackSelectedCommand { get; private set; }

        private void OnTitleSelected(MusicItem music)
        {
            NavigationService.Navigate("PlayerPage", JsonConvert.SerializeObject(MusicDataContainer.Instance.Initialize(music)));
        }
    }
}
