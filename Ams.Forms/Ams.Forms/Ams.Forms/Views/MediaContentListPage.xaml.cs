using Ams.Forms.DataServices;
using Ams.Forms.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Ams.Forms.DataServices;
using Ams.Forms.Entities;

namespace Ams.Forms.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MediaContentListPage : ContentPage
	{

        private readonly string _mediaServiceStorage = "";
        public List<MediaContentModel> MediaContent { get; set; }
        public MediaContentListPage()
		{
            //TODO: Not best solution, till i can figure out how, this will have to do
            BindingContext = null;

            InitializeComponent();

            SyncWithAzure();

            var db = new MediaServicesDb();

            MediaContent = db.GetMediaContent();

            BindingContext = this;
		}
        
        public async Task Handle_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var selected = e.Item as MediaContentModel;

            await Navigation.PushAsync(new PlayMediaContentPage(selected.MediaName, selected.MediaUri));
        }

        private void SyncWithAzure()
        {
            var mediaContent = Retreive();

            var db = new MediaServicesDb();

            db.InsertContent(mediaContent);
        }

        public List<MediaContentModel> Retreive()
        {
            var storageAccount = CloudStorageAccount.Parse(_mediaServiceStorage);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("mediaassets");

            var tableOperation = new TableQuery<MediaContentEntity>();
            var mediaList = table.ExecuteQuerySegmentedAsync(tableOperation, null).Result;

            var mediaContent = new List<MediaContentModel>();
            foreach (var item in mediaList)
            {
                var content = new MediaContentModel
                {
                    MediaName = item.PartitionKey,
                    MediaUri = item.UriSmoothStreaming
                };
                mediaContent.Add(content);
            }

            return mediaContent;
        }

        public async Task Refresh()
        {
            await Navigation.PushAsync(new MediaContentListPage());
        }
    }
}