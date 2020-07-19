using Stylet;
using System.Collections.Generic;
using YouTubeClient.Client.Models;

namespace YouTubeClient.Client.ViewModels
{
    public class VideoDetailsViewModel : Screen
    {
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

        public VideoDetailsViewModel()
        {
            this.DisplayName = "Video Details";
        }

        public VideoDetailsViewModel(IEnumerable<VideoDetails> videoDetails)
        {
            VideoDetailsOutput = videoDetails;
        }
    }
}
