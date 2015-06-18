using System;

namespace OnJamendo.Model
{
    public class Track : MusicItem, IEquatable<Track>
    {
        private string _albumName;
// ReSharper disable InconsistentNaming
        public string Album_Name
// ReSharper restore InconsistentNaming
        {
            get { return _albumName; }
            set { SetProperty(ref _albumName, value); }
        }

        private string _albumImage;
// ReSharper disable InconsistentNaming
        public string Album_Image
// ReSharper restore InconsistentNaming
        {
            get { return _albumImage; }
            set { SetProperty(ref _albumImage, value); }
        }

       /* private string _laregeAlbumImage;
        public string LaregeAlbumImage
        {
            get { return _laregeAlbumImage; }
            set { SetProperty(ref _laregeAlbumImage, value, "LaregeAlbumImage"); }
        }*/

        private string _artistName;
// ReSharper disable InconsistentNaming
        public string Artist_Name
// ReSharper restore InconsistentNaming
        {
            get { return _artistName; }
            set { SetProperty(ref _artistName, value); }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private string _stream;
        public string Stream
        {
            get { return _stream; }
            set { SetProperty(ref _stream, value); }
        }

        public override int TrackCount
        {
            get { return 1; }
            set { }
        }


        public Track() { }
        protected Track(Track track)
        {
            _albumName = track._albumName;
            _albumImage = track._albumImage;
            _artistName = track._artistName;
            _stream = track._stream;
            _text = track._text;
        }

        #region Equality members
        public bool Equals(Track other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && string.Equals(_stream, other._stream) && string.Equals(_albumImage, other._albumImage) && string.Equals(_albumName, other._albumName) && string.Equals(_artistName, other._artistName) && string.Equals(_text, other._text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Track) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (_stream != null ? _stream.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_albumImage != null ? _albumImage.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_albumName != null ? _albumName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_artistName != null ? _artistName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_text != null ? _text.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Track left, Track right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Track left, Track right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
