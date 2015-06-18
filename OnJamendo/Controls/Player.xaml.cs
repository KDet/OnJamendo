using System;
using System.Collections.Generic;
using OnJamendo.Model;
using Windows.Media;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236

namespace OnJamendo.Controls
{
    public sealed partial class Player : UserControl
    {
        public Player()
        {
            InitializeComponent();
            MediaControl.PlayPauseTogglePressed += MediaControl_PlayPauseTogglePressed;
            MediaControl.PlayPressed += MediaControl_PlayPressed;
            MediaControl.PausePressed += MediaControl_PausePressed;
            MediaControl.StopPressed += MediaControl_StopPressed;
            MediaControl.PreviousTrackPressed +=MediaControl_PreviousTrackPressed;
            MediaControl.NextTrackPressed +=MediaControl_NextTrackPressed;

            PlayButton.Click += MediaControl_PlayPauseTogglePressed;
            MediaEl.CurrentStateChanged += mediaElement_CurrentStateChanged;
            MediaEl.MediaOpened += mediaElement_MediaOpened;
            MediaEl.MediaEnded += mediaElement_MediaEnded;
            NextBtn.Click += MediaControl_NextTrackPressed;
            PrevBtn.Click += MediaControl_PreviousTrackPressed;
        }

        private int _currentSongIndex;
        private bool _previousRegistered;
        private bool _nextRegistered;
        private bool _wasPlaying;
        private IList<Track> _playList;

        #region DependencyProperties
        public Track Track
        {
            get { return (Track)GetValue(TrackProperty); }
            set { SetValue(TrackProperty, value); }
        }

        public DependencyProperty TrackProperty =
            DependencyProperty.Register("Track", typeof (Track),
                                        typeof (Player),
                                        new PropertyMetadata(null, TrackChangedCallback));

        private static async void TrackChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await Window.Current.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var player = (Player) d;
                    var newTrack = e.NewValue as Track;
                    if (newTrack == null) return;

                    player.MediaEl.Source = new Uri(newTrack.Stream);

                    MediaControl.TrackName = newTrack.Name;
                    MediaControl.ArtistName = newTrack.Artist_Name;
                    //TODO art
                    //MediaControl.AlbumArt = new Uri(newTrack.Image);
                });
        }

        public IList<Track> PlayListSource
        {
            get { return (IList<Track>)GetValue(PlayListSourceProperty); }
            set { SetValue(PlayListSourceProperty, value); }
        }

        public  DependencyProperty PlayListSourceProperty =
            DependencyProperty.Register("PlayListSource",
                                        typeof(IList<Track>),
                                        typeof (Player),
                                        new PropertyMetadata(null, PlayListSourceChangedCallback));

        private static async void PlayListSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await Window.Current.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var player = (Player) d;
                    var newPlayList = e.NewValue as IList<Track>;
                    if (newPlayList == null) return;
                    player._playList = newPlayList;
                });
        }

        public int PlayListIndex
        {
            get { return (int)GetValue(PlayListIndexProperty); }
            set { SetValue(PlayListIndexProperty, value); }
        }

        public DependencyProperty PlayListIndexProperty =
            DependencyProperty.Register("PlayListIndex", typeof (int),
                                        typeof (Player),
                                        new PropertyMetadata(null , PlayListIndexChangedCallback));

        private static async void PlayListIndexChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await Window.Current.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var player = (Player)d;
                    var songIndex = e.NewValue is int ? (int)e.NewValue : -1;
                    if (songIndex == -1) return;
                    player._currentSongIndex = songIndex;
                    player.MediaEl.Source = new Uri(player._playList[songIndex].Stream);
                });
        }

      //  //public TimeSpan Position
      //  //{
      //  //    get { return MediaEl.Position; }
      //  //    set { MediaEl.Position = value; }
      //  //}

      //  public TimeSpan CurrentPosition
      //  {
      //      get { return (TimeSpan)MediaEl.GetValue(MediaElement.PositionProperty); }
      //      set { MediaEl.SetValue(MediaElement.PositionProperty, value); }
      //  }

      ////  public DependencyProperty CurrentPositionProperty = MediaElement.PositionProperty;

        #endregion

        //public class Song
        //{
        //    public StorageFile File;
        //    public string Artist;
        //    public string Track;
        //    //public Windows.Storage.FileProperties.StorageItemThumbnail AlbumArt;

        //    public Song(StorageFile file)
        //    {
        //        File = file;
        //    }

        //    public async Task GetMusicPropertiesAsync()
        //    {
        //        var properties = await File.Properties.GetMusicPropertiesAsync();
        //        //Windows.Storage.FileProperties.StorageItemThumbnail thumbnail = null;


        //        Artist = properties.Artist;
        //        Track = properties.Title;
        //    }
        //}

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (_currentSongIndex >= _playList.Count - 1) return;

            _currentSongIndex++;
            SetCurrentPlayingAsync(_currentSongIndex);
            if (_wasPlaying)
                MediaEl.Play();
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            PrevBtn.IsEnabled = PreviousTrackEnabled();
            NextBtn.IsEnabled = NextTrackEnabled();
            if (_wasPlaying)
                MediaEl.Play();
        }

        private bool PreviousTrackEnabled()
        {
            if (_currentSongIndex > 0)
            {
                if (!_nextRegistered)
                {
                    MediaControl.NextTrackPressed += MediaControl_NextTrackPressed;
                    _nextRegistered = true;
                }
                return true;
            }
            MediaControl.PreviousTrackPressed -= MediaControl_PreviousTrackPressed;
            return false;
            //SetCurrentPlayingAsync(_currentSongIndex);
        }

        private bool NextTrackEnabled()
        {
            if (_currentSongIndex < _playList.Count - 1 && _currentSongIndex >= 0)
            {
                if (!_previousRegistered)
                {
                    MediaControl.PreviousTrackPressed += MediaControl_PreviousTrackPressed;
                    _previousRegistered = true;
                }
                return true;
            }

           // if (!_nextRegistered)
           // {
                MediaControl.NextTrackPressed -= MediaControl_NextTrackPressed;
          //  }
            return false;
        }

        private void mediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (MediaEl.CurrentState == MediaElementState.Playing)
            {
                MediaControl.IsPlaying = true;
                PlayButton.Content = "\xE103";
            }
            else
            {
                MediaControl.IsPlaying = false;
                PlayButton.Content = "\xE102";
            }
           
        }

        //private async Task SetMediaElementSourceAsync(Song song)
        //{
        //    var stream = await song.File.OpenAsync(FileAccessMode.Read);
        //    await
        //        Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //                            () => MediaEl.SetSource(stream, song.File.ContentType));

        //    MediaControl.ArtistName = song.Artist;
        //    MediaControl.TrackName = song.Track;

        //}

//        private async Task CreatePlaylist(IReadOnlyCollection<StorageFile> files)
//        {

//            _playlistCount = files.Count;
//            if (_previousRegistered)
//            {
//                MediaControl.PreviousTrackPressed -= MediaControl_PreviousTrackPressed;
//                _previousRegistered = false;
//            }
//            if (_nextRegistered)
//            {
//                MediaControl.NextTrackPressed -= MediaControl_NextTrackPressed;
//                _nextRegistered = false;
//            }
//            _currentSongIndex = 0;
//            _playlist.Clear();

//            if (files.Count > 0)
//            {
//// Application now has read/write access to the picked file(s) 
//                if (files.Count > 1)
//                {
//                    MediaControl.NextTrackPressed += MediaControl_NextTrackPressed;
//                    _nextRegistered = true;
//                }

//                // Create the playlist
//                foreach (var newSong in files.Select(file => new Song(file)))
//                {
//                    await newSong.GetMusicPropertiesAsync();
//                    _playlist.Add(newSong);
//                }
//            }
            
//        }

        private void SetCurrentPlayingAsync(int playlistIndex)
        {
            //string errorMessage = null;
            _wasPlaying = MediaControl.IsPlaying;
            if (playlistIndex < 0 || playlistIndex >= _playList.Count) return;
            PlayListIndex = playlistIndex;
            
           // MediaEl.Source = new Uri(_playList[playlistIndex].Url); 

            //try
            //{
            //    var stream =
            //        await _playlist[playlistIndex].File.OpenAsync(FileAccessMode.Read);
            //    await
            //        Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            //                            () => MediaEl.SetSource(stream, _playlist[playlistIndex].File.ContentType));
            //}
            //catch (Exception e)
            //{
            //    errorMessage = e.Message;
            //}

            //if (errorMessage != null)
            //{
            //   // await DispatchMessage("Error" + errorMessage);
            //}
            MediaControl.ArtistName = _playList[playlistIndex].Artist_Name;
            MediaControl.TrackName = _playList[playlistIndex].Name;
        }

        private async void MediaControl_PreviousTrackPressed(object sender, object e)
        {
           // if (_currentSongIndex <= 0) return;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _currentSongIndex--;
                    SetCurrentPlayingAsync(_currentSongIndex);
                    PrevBtn.IsEnabled = PreviousTrackEnabled();
                    NextBtn.IsEnabled = NextTrackEnabled();
                });

            //if (_currentSongIndex >= _playList.Count - 1)
            //{
            //    if (!_nextRegistered)
            //    {
            //        MediaControl.NextTrackPressed += MediaControl_NextTrackPressed;

            //        NextBtn.IsEnabled = _nextRegistered = true;
            //    }
            //}


            //if (_currentSongIndex <= 0)
            //{
            //    MediaControl.PreviousTrackPressed -= MediaControl_PreviousTrackPressed;
            //    PrevBtn.IsEnabled = _previousRegistered = false;
            //}


        }

        private async void MediaControl_NextTrackPressed(object sender, object e)
        {
            //if (_currentSongIndex >= _playList.Count - 1) return;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _currentSongIndex++;
                    SetCurrentPlayingAsync(_currentSongIndex);
                    PrevBtn.IsEnabled = PreviousTrackEnabled();
                    NextBtn.IsEnabled = NextTrackEnabled();
                });
            //if (_currentSongIndex > 0)
            //{
            //    if (!_previousRegistered)
            //    {
            //        MediaControl.PreviousTrackPressed += MediaControl_PreviousTrackPressed;
            //        PrevBtn.IsEnabled = _previousRegistered = true;
            //    }
            //}
            //if (_currentSongIndex <= _playList.Count - 1) return;
            //if (!_nextRegistered) return;

            //MediaControl.NextTrackPressed -= MediaControl_NextTrackPressed;
            //NextBtn.IsEnabled = _nextRegistered = false;
        }

        private async void MediaControl_StopPressed(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MediaEl.Stop());
        }

        private async void MediaControl_PausePressed(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MediaEl.Pause());
        }

        private async void MediaControl_PlayPressed(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MediaEl.Play());
        }

        private async void MediaControl_PlayPauseTogglePressed(object sender, object e)
        {
            if (MediaControl.IsPlaying)
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MediaEl.Pause());
            else
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MediaEl.Play());
        }

        //private async void MediaControlOnPreviousTrackPressed(object sender, object o)
        //{
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MediaControl_PreviousTrackPressed(sender, o));
        //}

        //private async void MediaControlOnNextTrackPressed(object sender, object o)
        //{
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MediaControl_NextTrackPressed(sender, o));
        //}
    }

}
