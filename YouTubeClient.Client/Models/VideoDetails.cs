namespace YouTubeClient.Client.Models
{
    public class VideoDetails
    {
        public string VideoName { get; set; }
        public string VideoUrl { get; set; }
        private const string _baseVideoUrl = "http://www.youtube.com/embed/";

        public VideoDetails(string videoName, string videoId)
        {
            VideoName = videoName;
            VideoUrl = _baseVideoUrl + videoId;
        }
    }
}
