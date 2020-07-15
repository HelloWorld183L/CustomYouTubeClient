using System.Configuration;

namespace YouTubeClient.Client
{
    public class Credentials
    {
        public string ApiKey { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Username { get; }

        public Credentials()
        {
            ApiKey = ConfigurationManager.AppSettings["YouTubeAPIKey"];
            ClientId = ConfigurationManager.AppSettings["ClientId"];
            ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            Username = ConfigurationManager.AppSettings["Username"];
        }
    }
}
