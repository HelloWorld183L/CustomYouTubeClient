using Google.Apis.YouTube.v3.Data;
using Stylet;
using System.Collections.Generic;
using System.Threading.Tasks;
using YouTubeClient.Client.Models;

namespace YouTubeClient.Client.ViewModels
{
    public class ShellViewModel : Screen
    {
        private Client _youtubeClient;
        private IWindowManager _windowManager;
        private IEnumerable<Subscription> _subscriptionData;
        public IEnumerable<Subscription> SubscriptionData 
        {
            get { return _subscriptionData;  }
            set 
            {
                _subscriptionData = value;
                NotifyOfPropertyChange(nameof(SubscriptionData));
            }
        }
        public Subscription SelectedSubscription { get; set; }

        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public async Task LoadSubscriptionData()
        {
            var createClientTask = Client.CreateAsync();
            createClientTask.Wait();
            _youtubeClient = createClientTask.Result;
            var subscriptions = await _youtubeClient.GetSubscriptionsAsync();
            SubscriptionData = subscriptions;
        }

        public async Task DisplayVideoDetails()
        {
            var channel = await _youtubeClient.GetChannelAsync(SelectedSubscription.Snippet.ResourceId.ChannelId);
            
            var videoDetails = new List<VideoDetails>();
            var playlistItems = await _youtubeClient.GetPlaylistItemsAsync(channel.ContentDetails.RelatedPlaylists.Uploads);
            foreach (var playlistItem in playlistItems)
            {
                var videoDetailsItem = new VideoDetails(playlistItem.Snippet.Title,
                                                        playlistItem.ContentDetails.VideoId);
                videoDetails.Add(videoDetailsItem);
            }

            var videoDetailsViewModel = new VideoDetailsViewModel(videoDetails);
            _windowManager.ShowWindow(videoDetailsViewModel);
        }
    }
}