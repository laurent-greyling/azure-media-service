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

namespace Ams.Forms.Droid
{
    [Activity(Label = "Ams.Forms.Android", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            SyncWithAzure();

            LoadApplication(new App());
        }

        private void SyncWithAzure()
        {
            var db = new MediaServicesDb();

            var addMediaContent = new List<MediaContentModel>
            {
                new MediaContentModel { MediaName = "Name1", MediaUri="uri1" },
                new MediaContentModel { MediaName = "Name2", MediaUri="uri2" }
            };
            
            db.InsertContent(addMediaContent);            
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
