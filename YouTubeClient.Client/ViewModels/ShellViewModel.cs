using Google.Apis.YouTube.v3.Data;
using Stylet;
using System.Collections.Generic;

namespace YouTubeClient.Client.ViewModels
{
    public class ShellViewModel : Screen
    {
        private Client _youtubeClient;
        private IWindowManager _windowManager;
        private const int defaultMaxResults = 20;
        private string nextPageToken = "";
        private string previousPageToken = "";

        private int _currentProgress;
        private int _pageNumber;
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
        public int CurrentProgress 
        { 
            get { return _currentProgress; }
            set
            {
                _currentProgress = value;
                NotifyOfPropertyChange(nameof(CurrentProgress));
            }
        }
        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                _pageNumber = value;
                NotifyOfPropertyChange(nameof(PageNumber));
            }
        }

        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            var createClientTask = Client.CreateAsync();
            createClientTask.Wait();
            _youtubeClient = createClientTask.Result;

            PageNumber = 1;
        }

        public void NextPage()
        {
            var subscriptionListResponse = _youtubeClient.GetSubscriptions(nextPageToken, defaultMaxResults);
            nextPageToken = subscriptionListResponse.NextPageToken;
            previousPageToken = subscriptionListResponse.PrevPageToken;
            PageNumber++;

            SubscriptionData = subscriptionListResponse.Items;
        }

        public void PreviousPage()
        {
            var subscriptionListResponse = _youtubeClient.GetSubscriptions(previousPageToken, defaultMaxResults);
            previousPageToken = subscriptionListResponse.PrevPageToken;
            nextPageToken = subscriptionListResponse.NextPageToken;
            if (PageNumber > 1) PageNumber--;

            SubscriptionData = subscriptionListResponse.Items;
        }

        public void LoadSubscriptionData()
        {
            var subscriptionListResponse = _youtubeClient.GetSubscriptions("", defaultMaxResults);
            nextPageToken = subscriptionListResponse.NextPageToken;

            SubscriptionData = subscriptionListResponse.Items;
        }

        public void DisplayVideoDetails()
        {
            var channelId = SelectedSubscription.Snippet.ResourceId.ChannelId;
            var videoDetailsResponse = _youtubeClient.GetVideoDetails("",
                                       channelId);

            var videoDetailsViewModel = new VideoDetailsViewModel(videoDetailsResponse.Items, _youtubeClient,
                                        channelId);
            _windowManager.ShowWindow(videoDetailsViewModel);
        }
    }
}