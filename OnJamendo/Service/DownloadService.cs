using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OnJamendo.Model;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace OnJamendo.Service
{
    public class ProgressDowloadFileEventArgs 
    {
        public ulong RecivedBytes { get; set; }
        public ulong TotalBytesToRecive { get; set; }
        public DownloadTrack Track { get; private set; }

        public ProgressDowloadFileEventArgs(DownloadTrack track, ulong recived, ulong total) 
        {
            RecivedBytes = recived;
            TotalBytesToRecive = total;
            Track = track;
        }
    }

    public class DownloadService
    {
        private static DownloadService _instance;

        public static DownloadService Current
        {
            get { return _instance ?? (_instance = new DownloadService()); }
        }

        #region Events

        public event EventHandler<DownloadTrack> AddToDownloads;
        public event EventHandler<DownloadTrack> RemoveFromDownloads;
        public event EventHandler<ProgressDowloadFileEventArgs> ProgressChanged;

        protected virtual void OnProgressChanged(ProgressDowloadFileEventArgs e)
        {
            var handler = ProgressChanged;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnRemoveFromDownloads(DownloadTrack e)
        {
            var handler = RemoveFromDownloads;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnAddToDownloads(DownloadTrack e)
        {
            var handler = AddToDownloads;
            if (handler != null) handler(this, e);
        }

        #endregion


        private DownloadOperation _downloadOperation;
        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();
        private SynchronizationContext _synchronizationContext;

        public DownloadTrack CurrentTrack { get; set; }

        public async Task DownloadAsunc(Track track)
        {
            if(track == null) return;
            var saveFile = await PickSaveFaleStorage(track);
            if(saveFile == null) return;
            CurrentTrack = new DownloadTrack(track, saveFile);

           // OnAddToDownloads(CurrentTrack);

            if (_downloadOperation != null && _downloadOperation.Progress.Status != BackgroundTransferStatus.Canceled)
                return;
            try
            {
                var downloader = new BackgroundDownloader();
                _downloadOperation = downloader.CreateDownload(new Uri(track.Url), saveFile);
                var progressCallback = new Progress<DownloadOperation>(OnDownloaderProgress);
                _synchronizationContext = SynchronizationContext.Current;
                //_cancellationSource = new CancellationTokenSource();
                await _downloadOperation.StartAsync().AsTask(_cancellationSource.Token, progressCallback);
             //   OnRemoveFromDownloads(CurrentTrack);

            }
            catch (Exception)
            {
                    
                throw;
            }
        }

        private void OnDownloaderProgress(DownloadOperation downloadOperation)
        {
            _synchronizationContext.Post(state => OnProgressChanged(new ProgressDowloadFileEventArgs(CurrentTrack,downloadOperation.Progress.BytesReceived, downloadOperation.Progress.TotalBytesToReceive)), null);
        }

        private static async Task<StorageFile> PickSaveFaleStorage(Track track)
        {
            try
            {
                var picker = new FileSavePicker
                    {
                        SuggestedStartLocation = PickerLocationId.MusicLibrary,
                        SuggestedFileName = string.Format("{0} - {1}", track.Name, track.Artist_Name)
                    };
                picker.FileTypeChoices.Add("mp3 file", new List<string>{".mp3"});
                picker.DefaultFileExtension = ".mp3";
                var file = await picker.PickSaveFileAsync();
                return file;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    
}
