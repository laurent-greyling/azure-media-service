using Ams.Forms.DataServices;
using Ams.Forms.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ams.Forms.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MediaContentListPage : ContentPage
	{
        public List<MediaContentModel> MediaContent { get; set; }
        public MediaContentListPage()
		{
			InitializeComponent();

            var db = new MediaServicesDb();

            MediaContent = db.GetMediaContent();

            BindingContext = this;
		}
	}
}