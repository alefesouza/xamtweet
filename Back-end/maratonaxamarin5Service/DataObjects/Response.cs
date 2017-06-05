using System.Collections.Generic;

namespace maratonaxamarin5Service.DataObjects
{
    public class Response
    {
        public bool More_tweets { get; set; }
        public List<Tweet> Tweets { get; set; } 
    }
}