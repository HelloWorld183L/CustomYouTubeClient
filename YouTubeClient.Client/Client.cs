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
        private string _refreshToken;

        public static Task<Client> CreateAsync()
        {
            var client = new Client();
            return client.SetUpClientAsync();
        }

        public async Task DownloadVideosAsync(int channelId)
        {

        }

        public async Task<IList<Subscription>> GetSubscriptionsAsync(int numberOfSubscriptions)
        {
            var getSubscriptionsRequest = _youtubeService.Subscriptions.List("");
            getSubscriptionsRequest.Mine = true;
            getSubscriptionsRequest.MaxResults = numberOfSubscriptions;
            getSubscriptionsRequest.AccessToken = _accessToken;

            var subscriptions = getSubscriptionsRequest.Execute();
            return subscriptions.Items;
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
            _refreshToken = credential.Token.RefreshToken;
            return new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApiKey = _credentials.ApiKey,
                ApplicationName = "YouTubeClient",
            });
        }
    }
}