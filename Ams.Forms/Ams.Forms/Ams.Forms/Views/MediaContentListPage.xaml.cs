using Ams.Forms.DataServices;
using Ams.Forms.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

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
        
        public async Task Handle_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var selected = e.Item as MediaContentModel;

            await Navigation.PushAsync(new PlayMediaContentPage(selected.MediaName, selected.MediaUri));
        }
    }
}