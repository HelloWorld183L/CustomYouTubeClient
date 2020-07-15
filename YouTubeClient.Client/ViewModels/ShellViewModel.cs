using Google.Apis.YouTube.v3.Data;
using Stylet;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace YouTubeClient.Client.Pages
{
    public class ShellViewModel : Conductor<IScreen>
    {
        public IList<Subscription> SubscriptionData { get; set; }
        private const int numOfSubscriptions = 5;

        public async Task LoadSubscriptionData()
        {
            var createClientTask = Client.CreateAsync();
            createClientTask.Wait();
            var youtubeClient = createClientTask.Result;

            SubscriptionData = await youtubeClient.GetSubscriptionsAsync(numOfSubscriptions);
            Debug.WriteLine(SubscriptionData.Count);
        }
    }
}
