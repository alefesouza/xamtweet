namespace XamTweet.Models
{
    public class TwitterUser
    {
        private string _screenName;

        public string Id_str { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public object Url { get; set; }
        public int Followers_count { get; set; }
        public int Friends_count { get; set; }
        public int Listed_count { get; set; }
        public string Created_at { get; set; }
        public int Favourites_count { get; set; }
        public bool Verified { get; set; }
        public int Statuses_count { get; set; }
        public string Profile_background_color { get; set; }
        public string Profile_image_url { get; set; }
        public string Profile_image_url_https { get; set; }
        public string Profile_banner_url { get; set; }
        public string Profile_link_color { get; set; }
        public bool Following { get; set; }

        public string Screen_name
        {
            get => "@" + _screenName;
            set
            {
                _screenName = value;
            }
        }
    }
}