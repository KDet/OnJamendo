using System;

namespace OnJamendo.Model
{
    public class MusicDataContainer
    {
        public MusicItem MusicItem { get; set; }
        public Track SpecialTrack { get; set; }
        public Type Type { get;  set; }

        public MusicDataContainer() { }

        public MusicDataContainer(MusicItem musicItem)
        {
            Initialize(musicItem);
        }

        public MusicDataContainer Initialize(MusicItem musicItem)
        {
            Type = musicItem.GetType();
            MusicItem = musicItem;
            SpecialTrack = musicItem is Track ? (Track)musicItem : null;
            return this;
        }
        private static MusicDataContainer _instance;

        public static MusicDataContainer Instance
        {
            get { return _instance ?? (_instance = new MusicDataContainer()); }
        }
    }
}
