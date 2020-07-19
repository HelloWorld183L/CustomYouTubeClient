using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace YouTubeClient.Client
{
    public class Client
    {
        private Credentials _credentials;
        private string[] _scope;
        private YouTubeService _youtubeService;
        private string _accessToken;
        private const int maxVideoCount = 20;
        private const int numOfSubscriptions = 20;

        public async Task<IEnumerable<PlaylistItem>> GetPlaylistItemsAsync(string playlistId)
        {
            var getPlaylistItemsRequest = _youtubeService.PlaylistItems.List("contentDetails, snippet");
            getPlaylistItemsRequest.MaxResults = maxVideoCount;
            getPlaylistItemsRequest.PlaylistId = playlistId;
            
            var playlistItems = getPlaylistItemsRequest.Execute().Items;
            return playlistItems;
        }

        public async Task<Channel> GetChannelAsync(string channelId)
        {
            var getChannelRequest = _youtubeService.Channels.List("contentDetails");
            getChannelRequest.Id = channelId;

            var channel = getChannelRequest.Execute().Items[0];
            return channel;
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync()
        {
            var getSubscriptionRequest = _youtubeService.Subscriptions.List("snippet");
            getSubscriptionRequest.Mine = true;
            getSubscriptionRequest.MaxResults = numOfSubscriptions;

            var subscriptions = getSubscriptionRequest.Execute();
            return subscriptions.Items;
        }

        public static Task<Client> CreateAsync()
        {
            var client = new Client();
            return client.SetUpClientAsync();
        }

        private async Task<Client> SetUpClientAsync()
        {
            _credentials = new Credentials();
            _scope = new[] { YouTubeService.Scope.YoutubeReadonly };
            _youtubeService = await GetYouTubeServiceAsync();
            return this;
        }

        private async Task<YouTubeService> GetYouTubeServiceAsync()
        {
            var clientSecrets = new ClientSecrets()
            {
                ClientId = _credentials.ClientId,
                ClientSecret = _credentials.ClientSecret
            };
            
            var credential =
                await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, _scope, _credentials.Username,
                                                                  CancellationToken.None, new FileDataStore("YouTubeClient"));

            _accessToken = credential.Token.AccessToken;
            return new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApiKey = _credentials.ApiKey,
                ApplicationName = "YouTubeClient",
            });
        }
    }
}