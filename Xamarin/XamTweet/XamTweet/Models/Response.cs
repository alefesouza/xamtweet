using System.Collections.Generic;

namespace XamTweet.Models
{
    public class Response
    {
        public bool More_tweets { get; set; }
        public List<Tweet> Tweets { get; set; }
    }
}
