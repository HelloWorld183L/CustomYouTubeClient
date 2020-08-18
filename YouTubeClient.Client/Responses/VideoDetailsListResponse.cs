using System.Collections.Generic;
using YouTubeClient.Client.Models;

namespace YouTubeClient.Client.Responses
{
    public class VideoDetailsListResponse
    {
        public IEnumerable<VideoDetails> Items { get; set; }
        public string NextPageToken { get; set; }
        public string PreviousPageToken { get; set; }
    }
}
