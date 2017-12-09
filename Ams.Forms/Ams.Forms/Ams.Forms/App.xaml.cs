using Ams.Forms.Views;
using Xamarin.Forms;

namespace Ams.Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new MediaContentListPage();
            else
                MainPage = new NavigationPage(new MediaContentListPage());
        }
    }
}