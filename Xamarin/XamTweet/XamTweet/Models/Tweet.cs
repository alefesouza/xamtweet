namespace XamTweet.Models
{
    public class Tweet
    {
        public string Created_at { get; set; }
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