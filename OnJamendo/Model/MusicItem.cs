using System;
using OnJamendo.Common;

namespace OnJamendo.Model
{
    public class MusicItem : BindableBase, IEquatable<MusicItem>
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _image;
        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }
        private int _duration;
        public int Duration
        {
            get { return _duration; }
            set { SetProperty(ref _duration, value); }
        }
        private int _trackCount;
        public virtual int TrackCount
        {
            get { return _trackCount; }
            set { SetProperty(ref _trackCount, value); }
        }
        private string _url;
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        #region Equality members
        public bool Equals(MusicItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_url, other._url) && _trackCount == other._trackCount && _duration == other._duration && string.Equals(_image, other._image) && string.Equals(_name, other._name) && string.Equals(_id, other._id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MusicItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_url != null ? _url.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _trackCount;
                hashCode = (hashCode*397) ^ _duration;
                hashCode = (hashCode*397) ^ (_image != null ? _image.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (_name != null ? _name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _id;
                return hashCode;
            }
        }

        public static bool operator ==(MusicItem left, MusicItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MusicItem left, MusicItem right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
