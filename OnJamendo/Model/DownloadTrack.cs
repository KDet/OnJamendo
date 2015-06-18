using Windows.Storage;

namespace OnJamendo.Model
{
    public class DownloadTrack :Track
    {
        private ulong _totalBytesToRecive;
        public ulong TotalBytesToRecive
        {
            get { return _totalBytesToRecive; }
            set { SetProperty(ref _totalBytesToRecive, value); }
        }

        private ulong _recivedBytes;
        public ulong RecivedBytes
        {
            get { return _recivedBytes; }
            set { SetProperty(ref _recivedBytes, value); }
        }

        private StorageFile _storageFile;
        public StorageFile StorageFile
        {
            get { return _storageFile; }
            set { SetProperty(ref _storageFile, value); }
        }

        public DownloadTrack(Track track, StorageFile storageFile)
            : base(track)
        {
            StorageFile = storageFile;
        }

        public DownloadTrack() { }
    }
}
