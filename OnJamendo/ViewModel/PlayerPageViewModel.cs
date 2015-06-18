using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using OnJamendo.Common;
using OnJamendo.Model;
using OnJamendo.Repository;
using OnJamendo.Service;
using Windows.UI.Popups;

namespace OnJamendo.ViewModel
{
    public class PlayerPageViewModel : BaseViewModel
    {
        public PlayerPageViewModel()
        {
            #region todell

            //AllTracks = new ObservableCollection<Track>
            //    {
            //        new Track
            //            {
            //                Album_Image = "https://imgjam.com/albums/s121/121766/covers/1.400.jpg",
            //                Album_Name = "album name",
            //                Artist_Name = "artist",
            //                Duration = 2567,
            //                Id = 25,
            //                Name = "track name",

            //            },
            //        new Track
            //            {
            //                Album_Image = "https://imgjam.com/albums/s121/121766/covers/1.400.jpg",
            //                Album_Name = "album name 2",
            //                Artist_Name = "artist 2",
            //                Duration = 256,
            //                Id = 21,
            //                Name = "track name2",
            //            },

            //        new Track
            //            {
            //                Album_Image = "https://imgjam.com/albums/s121/121766/covers/1.400.jpg",
            //                Album_Name = "album name 452",
            //                Artist_Name = "artist 4",
            //                Duration = 25,
            //                Id = 211,
            //                Name = "track name2",
            //            },

            //        new Track
            //            {
            //                Album_Image = "https://imgjam.com/albums/s121/121766/covers/1.400.jpg",
            //                Album_Name = "album name 12",
            //                Artist_Name = "artist 7",
            //                Duration = 1256,
            //                Id = 1,
            //                Name = "track name2",
            //            },

            //        new Track
            //            {
            //                Album_Image = "https://imgjam.com/albums/s121/121766/covers/1.400.jpg",
            //                Album_Name = "album name 2",
            //                Artist_Name = "artist 2",
            //                Duration = 256,
            //                Id = 21,
            //                Name = "track name2",
            //            },

            //        new Track
            //            {
            //                Album_Image = "https://imgjam.com/albums/s121/121766/covers/1.400.jpg",
            //                Album_Name = "album name 452",
            //                Artist_Name = "artist 4",
            //                Duration = 25,
            //                Id = 211,
            //                Name = "track name2",
            //            },

            //        new Track
            //            {
            //                Album_Image = "https://imgjam.com/albums/s121/121766/covers/1.400.jpg",
            //                Album_Name = "album name 12",
            //                Artist_Name = "artist 7",
            //                Duration = 1256,
            //                Id = 1,
            //                Name = "track name2",
            //            },
            //        new Track
            //            {
            //                Album_Image = "https://imgjam.com/albums/s121/121766/covers/1.400.jpg",
            //                Album_Name = "album name 452",
            //                Artist_Name = "artist 4",
            //                Duration = 25,
            //                Id = 211,
            //                Name = "track name2",
            //            },

            //        new Track
            //            {
            //                Album_Image = "https://imgjam.com/albums/s121/121766/covers/1.400.jpg",
            //                Album_Name = "album name 12",
            //                Artist_Name = "artist 7",
            //                Duration = 1256,
            //                Id = 1,
            //                Name = "track name2",
            //            }
            //    };

            #endregion

            DownloadTrack = new RelayCommand<Track>(OnDownloadTrack);
            TrackDownloadService.AddToDownloads += (sender, track) => DownloadTracks.Add(track);
            TrackDownloadService.RemoveFromDownloads += (sender, track) => DownloadTracks.Remove(track);
            TrackDownloadService.ProgressChanged += TrackDownloadService_ProgressChanged;

        }

        void TrackDownloadService_ProgressChanged(object sender, ProgressDowloadFileEventArgs e)
        {
            var progresTrack = DownloadTracks.FirstOrDefault(track => track.Equals(e.Track));
            if (progresTrack == null) return;
            progresTrack.RecivedBytes = e.RecivedBytes;
            progresTrack.TotalBytesToRecive = e.TotalBytesToRecive;
        }


        #region ObservableCollections

        private ObservableCollection<Track> _allTracks;
        public ObservableCollection<Track> AllTracks
        {
            get { return _allTracks; }
            set { SetProperty(ref _allTracks, value); }
        }

        private ObservableCollection<DownloadTrack> _downloadTracks;
        public ObservableCollection<DownloadTrack> DownloadTracks
        {
            get { return _downloadTracks; }
            set { SetProperty(ref _downloadTracks, value); }
        }

        #endregion


        private Track _curentTrack;
        public Track CurrentTrack
        {
            get { return _curentTrack; }
            set { SetProperty(ref _curentTrack, value); }
        }

        public override async void SaveState(Dictionary<string, object> pageState)
        {
            if (AllTracks != null)
                pageState["AllTracks"] = await JsonConvert.SerializeObjectAsync(AllTracks);

            if (CurrentTrack != null)
                pageState["CurrentTrack"] = JsonConvert.SerializeObject(CurrentTrack);
        }

        public override async void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            try
            {
                if (string.IsNullOrEmpty(navigationParameter as string)) return;
                if (pageState != null && pageState.Count != 0)
                {
                    if (pageState.ContainsKey("AllTracks"))
                        AllTracks = await JsonConvert.DeserializeObjectAsync<ObservableCollection<Track>>(pageState["AllTracks"] as string);
                    if (pageState.ContainsKey("CurrentTrack"))
                        CurrentTrack = JsonConvert.DeserializeObject<Track>(pageState["CurrentTrack"] as string);
                }
                if (AllTracks != null) return;
                var musicItem = JsonConvert.DeserializeObject<MusicDataContainer>(navigationParameter as string);

                if (musicItem.Type == typeof(Album))
                    AllTracks = await Repository.GetAlbumTracks(musicItem.MusicItem.Id);
                else if (musicItem.Type == typeof(PlayList))
                    AllTracks = await Repository.GetPlayListTracks(musicItem.MusicItem.Id);
                else if (musicItem.Type == typeof(Track))
                    AllTracks = await Repository.GetTopTracks(100);

                CurrentTrack = musicItem.SpecialTrack ?? AllTracks.FirstOrDefault();
            }
            catch (HttpRequestException)
            {
                new MessageDialog("Помилка підключення до інтернету", "Error").ShowAsync();
               // Notify("Відсутнє з'єднання з інтернетом", "Error");
            }
            
        }


        public RelayCommand<Track> DownloadTrack { get; private set; }

        private readonly DownloadService _trackDownloadService = DownloadService.Current;
        public DownloadService TrackDownloadService { get { return _trackDownloadService; } }
        
        private async void OnDownloadTrack(Track track)
        {
            await TrackDownloadService.DownloadAsunc(track);
        }


    }
}
