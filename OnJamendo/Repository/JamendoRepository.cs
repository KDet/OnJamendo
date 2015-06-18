using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnJamendo.Model;
using Windows.Networking.Connectivity;

namespace OnJamendo.Repository
{
    public class JamendoRepository : IJamendoRepository
    {
        public const string BaseUrl = "http://api.jamendo.com/get2/";
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        private readonly SynchronizationContext _syncContext;

        public event EventHandler ConnectionLost;
        public event EventHandler ConnectionFound;
        public JamendoRepository()
        {
            _syncContext= SynchronizationContext.Current;
            NetworkInformation.NetworkStatusChanged += sender =>
                {
                    ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
                    _syncContext.Post(state =>
                        {
                            if (profile == null)
                            {
                                if (ConnectionLost != null)
                                    ConnectionLost(this, EventArgs.Empty);
                            }
                            else
                            {
                                if (ConnectionFound != null)
                                    ConnectionFound(this, EventArgs.Empty);
                            }
                        }, null);
                };
        }

        public async Task<ObservableCollection<Track>> GetTopTracks(int count, ImageSize coversSize = ImageSize.Medium)
        {
            var tracksQuery =
                string.Format(
                    "id+name+image+duration+url+stream+text+album_name+album_image+artist_name/track/json/track_album+album_artist/?n={0}&order=ratingweek_desc", count);

            var tracks = await FetchDataAsync<Track>(tracksQuery);
            ResetCoversSize(tracks, coversSize);
            return tracks;
        }

        public async Task<ObservableCollection<Album>> GetTopAlbums(int count, ImageSize cover = ImageSize.Medium)
        {
            var albumsQuery =
                string.Format(
                    "id+name+image+duration+url+genre+artist_name/album/json/?n={0}&imagesize={1}&order=ratingweek_desc",
                    count, (int)cover);

            var albums = await FetchDataAsync<Album>(albumsQuery);
            await SetTrackApproximatelyCount(_httpClient, albums);
            return albums;
        }

        public async Task<ObservableCollection<PlayList>> GetTopPlayLists(int count, ImageSize coversSize = ImageSize.ZoomedLarge)
        {
            var playListsQuery =
                string.Format("id+name+url+image+duration/playlist/json/?n={0}&imagesize={1}&order=ratingweek_desc", count, (int)coversSize);

            var playLists = await FetchDataAsync<PlayList>(playListsQuery);
            await FetchPlayListImagesAsync(_httpClient, playLists, coversSize);
            await SetTrackApproximatelyCount(_httpClient, playLists);
            return playLists;
        }

        public async Task<ObservableCollection<Track>> GetAlbumTracks(int albumId, ImageSize coversSize = ImageSize.Medium)
        {
            var albumTracksQuery =
                string.Format(
                    "id+name+image+duration+url+stream+text+album_name+album_image+artist_name/track/json/track_album+album_artist/?album_id={0}&n=all&order=ratingweek_desc",
                    albumId);

            var tracks = await FetchDataAsync<Track>(albumTracksQuery);
            ResetCoversSize(tracks, coversSize);
            return tracks;
        }

        public async Task<ObservableCollection<Track>> GetPlayListTracks(int playListId, ImageSize coversSize = ImageSize.Medium)
        {
            var queryRequestNumb = "all";
            var conectCost = NetworkInformation.GetInternetConnectionProfile().GetConnectionCost();
            
            if (conectCost.NetworkCostType == NetworkCostType.Variable ||
                conectCost.NetworkCostType == NetworkCostType.Fixed && conectCost.ApproachingDataLimit || conectCost.Roaming)
                queryRequestNumb = "20";

            var playlistTracksQuery =
                string.Format(
                    "id+name+image+duration+url+stream+text+album_name+album_image+artist_name/track/json/playlist_track+track_album+album_artist/?playlist_id={0}&n={1}&order=ratingweek_desc",
                    playListId, queryRequestNumb);

            var playlists = await FetchDataAsync<Track>(playlistTracksQuery);
            ResetCoversSize(playlists, coversSize);
            return playlists;
        }

        #region Addition methods
        protected async Task<ObservableCollection<T>> FetchDataAsync<T>(string query) where T: MusicItem
        {
            try
            {
                var jsonResponse = await _httpClient.GetStringAsync(query);
                var dataList = await JsonConvert.DeserializeObjectAsync<ObservableCollection<T>>(jsonResponse);
                return dataList;
            }
            catch (Exception exception)
            {
                if(exception is HttpRequestException) throw;
                return null;
            }
        }

        //TODO переробити
        private static void ResetCoversSize(IEnumerable<Track> tracks, ImageSize appropriateSize)
        {
            var stringSize = ((int)appropriateSize).ToString();
            foreach (var track in tracks)
            {
                track.Album_Image = track.Album_Image.Replace("100", stringSize);
            }
        }

        private static async Task FetchPlayListImagesAsync(HttpClient client, IEnumerable<PlayList> playLists, ImageSize imageSize)
        {
            foreach (var playList in playLists)
            {
                var url =
                    string.Format("image/album/json/track_album+track_playlist/?playlist_id={0}&n=1&imagesize={1}",playList.Id, (int)imageSize);

                string data = await client.GetStringAsync(url);
                var array = JArray.Parse(data);
                if (array.Count > 0)
                    playList.Image = array.First.ToString();
            }
        }

        private static async Task SetTrackApproximatelyCount<T>(HttpClient client, IEnumerable<T> list, int approxFrige = -1) where T : MusicItem
        {
            string type = typeof(T).Name.ToLower();
            foreach (var item in list)
            {
                var url = string.Format("track_id/track/json/track_{0}/?{0}_id={1}&n={2}", type, item.Id, approxFrige < 0 ? "all" : approxFrige.ToString());

                var data = await client.GetStringAsync(url);
                item.TrackCount = JArray.Parse(data).Count;
            }
        }
        #endregion
    }
}
