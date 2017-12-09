using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Ams.Forms.DataServices;
using System.Collections.Generic;
using Ams.Forms.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Ams.Forms.Entities;

namespace Ams.Forms.Droid
{
    [Activity(Label = "Ams.Forms.Android", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly string _mediaServiceStorage = "";
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarians.MediaPlayer.Droid.VideoPlayerRenderer.Init(this);
            SyncWithAzure();

            LoadApplication(new App());
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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
