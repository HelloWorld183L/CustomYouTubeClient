using System.Threading.Tasks;
using Ookii.Dialogs.Wpf;

namespace YouTubeClient.Client.ViewModels
{
    public class DownloadVideosViewModel : Stylet.Screen
    {
        public string VideoCount { get; set; }
        public string ExportLocation { get; set; }
        private Client _youtubeClient = null;

        public async Task DownloadVideos()
        {
            if (_youtubeClient == null) _youtubeClient = await Client.CreateAsync();

            var subscriptions = await _youtubeClient.GetSubscriptionsAsync(int.Parse(VideoCount));
            foreach (var subscription in subscriptions)
            {
                var channelId = int.Parse(subscription.Snippet.ChannelId);
                await _youtubeClient.DownloadVideosAsync(channelId);
            }
        }

        public void OpenFileDialog()
        {
            var folderBrowserDialog = new VistaFolderBrowserDialog();
            folderBrowserDialog.Description = "Select the folder that you wish to export your videos to...";
            if (folderBrowserDialog.ShowDialog() == true)
            {
                ExportLocation = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
