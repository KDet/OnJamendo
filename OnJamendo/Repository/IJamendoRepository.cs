using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OnJamendo.Model;

namespace OnJamendo.Repository
{
    public interface IJamendoRepository
    {
        Task<ObservableCollection<Track>> GetTopTracks(int count, ImageSize coversSize = ImageSize.Medium);
        Task<ObservableCollection<Album>> GetTopAlbums(int count, ImageSize cover = ImageSize.Medium);
        Task<ObservableCollection<PlayList>> GetTopPlayLists(int count, ImageSize coversSize = ImageSize.ZoomedLarge);
        Task<ObservableCollection<Track>> GetAlbumTracks(int albumId, ImageSize coversSize = ImageSize.Medium);
        Task<ObservableCollection<Track>> GetPlayListTracks(int playListId, ImageSize coversSize = ImageSize.Medium);
        event EventHandler ConnectionLost;
        event EventHandler ConnectionFound;
    }
}