using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YouTubeClient.Client.Models;
using YouTubeClient.Client.Responses;

namespace YouTubeClient.Client
{
    public class Client
    {
        private Credentials _credentials;
        private string[] _scope;
        private YouTubeService _youtubeService;
        private const int maxVideoCount = 20;

        public VideoDetailsListResponse GetVideoDetails(string pageToken, string channelId)
        {
            var channel = GetChannel(channelId);
            var videoDetails = new List<VideoDetails>();
            var playlistItemsResponse = GetPlaylistItems(channel.ContentDetails.RelatedPlaylists.Uploads,
                                        pageToken);

            foreach (var playlistItem in playlistItemsResponse.Items)
            {
                var videoDetailsItem = new VideoDetails(playlistItem.Snippet.Title,
                                                        playlistItem.ContentDetails.VideoId);
                videoDetails.Add(videoDetailsItem);
            }

            var videoDetailsResponse = new VideoDetailsListResponse
            {
                Items = videoDetails,
                NextPageToken = playlistItemsResponse.NextPageToken,
                PreviousPageToken = playlistItemsResponse.PrevPageToken
            };
            return videoDetailsResponse;
        }

        private PlaylistItemListResponse GetPlaylistItems(string playlistId, string pageToken)
        {
            var getPlaylistItemsRequest = _youtubeService.PlaylistItems.List("contentDetails, snippet");
            getPlaylistItemsRequest.MaxResults = maxVideoCount;
            getPlaylistItemsRequest.PlaylistId = playlistId;
            getPlaylistItemsRequest.PageToken = pageToken;

            var playlistItemResponse = getPlaylistItemsRequest.Execute();
            return playlistItemResponse;
        }

        private Channel GetChannel(string channelId)
        {
            var getChannelRequest = _youtubeService.Channels.List("contentDetails");
            getChannelRequest.Id = channelId;

            var channel = getChannelRequest.Execute().Items[0];
            return channel;
        }

        public SubscriptionListResponse GetSubscriptions(string pageToken, int maxResults)
        {
            var getSubscriptionRequest = _youtubeService.Subscriptions.List("snippet");
            getSubscriptionRequest.Mine = true;
            getSubscriptionRequest.MaxResults = maxResults;
            getSubscriptionRequest.PageToken = pageToken;

            var subscriptionsResponse = getSubscriptionRequest.Execute();
            return subscriptionsResponse;
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

            return new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApiKey = _credentials.ApiKey,
                ApplicationName = "YouTubeClient",
            });
        }
    }
}