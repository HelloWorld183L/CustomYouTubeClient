using Stylet;
using System.Collections.Generic;
using YouTubeClient.Client.Models;

namespace YouTubeClient.Client.ViewModels
{
    public class VideoDetailsViewModel : Screen
    {
        private readonly Client _youtubeClient;
        private string _previousPageToken = "";
        private string _nextPageToken = "";
        private readonly string _channelId;
        private int _pageNumber;
        private IEnumerable<VideoDetails> _videoDetailsOutput;
        public IEnumerable<VideoDetails> VideoDetailsOutput
        {
            get { return _videoDetailsOutput; }
            set
            {
                _videoDetailsOutput = value;
                NotifyOfPropertyChange(nameof(VideoDetailsOutput));
            }
        }
        public int PageNumber 
        { 
            get { return _pageNumber;  }
            set
            {
                _pageNumber = value;
                NotifyOfPropertyChange(nameof(PageNumber));
            }
        }

        public VideoDetailsViewModel()
        {
            this.DisplayName = "Video Details";
            PageNumber = 1;
        }

        public VideoDetailsViewModel(IEnumerable<VideoDetails> videoDetails, Client client, string channelId)
        {
            VideoDetailsOutput = videoDetails;
            _youtubeClient = client;
            _channelId = channelId;
        }

        public void NextPage()
        {
            var videoDetails = _youtubeClient.GetVideoDetails(_nextPageToken, _channelId);
            _nextPageToken = videoDetails.NextPageToken;
            _previousPageToken = videoDetails.PreviousPageToken;
            PageNumber++;

            VideoDetailsOutput = videoDetails.Items;
        }

        public void PreviousPage()
        {
            var videoDetails = _youtubeClient.GetVideoDetails(_previousPageToken, _channelId);
            _nextPageToken = videoDetails.NextPageToken;
            _previousPageToken = videoDetails.PreviousPageToken;
            if (PageNumber > 1) PageNumber--;

            VideoDetailsOutput = videoDetails.Items;
        }
    }
}
