namespace OnJamendo.Model
{
    public class Album : MusicItem
    {
        private string _artistName;
// ReSharper disable InconsistentNaming
        public string Artist_Name
// ReSharper restore InconsistentNaming
        {
            get { return _artistName; }
            set { SetProperty(ref _artistName, value, "Artist_Name"); }
        }

        private string _genre;
        public string Genre
        {
            get { return _genre; }
            set { SetProperty(ref _genre, value, "Genre"); }
        }
    }
}
