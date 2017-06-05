using System;
using System.Globalization;

namespace maratonaxamarin5Service.DataObjects
{
    public class Tweet
    {
        private string _createdAt;

        public string Created_at {
            get => _createdAt;
            set {
                DateTime createdAt = DateTime.ParseExact(value, "ddd MMM dd HH:mm:ss +ffff yyyy", CultureInfo.InvariantCulture);
                createdAt = createdAt.AddHours(-3);
                _createdAt = createdAt.ToString("HH:mm dd/MM/yyyy");
            }
        }

        public string Id_str { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        public TwitterUser User { get; set; }
        public bool Is_quote_status { get; set; }
        public int Retweet_count { get; set; }
        public int Favorite_count { get; set; }
        public bool Favorited { get; set; }
        public bool Retweeted { get; set; }
    }
}