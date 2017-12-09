using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Ams.Forms.DataServices;

namespace Ams.Forms.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlayMediaContentPage : ContentPage
	{
        public string MediaFileName { get; set; }
        public string MediaUri { get; set; }



        public PlayMediaContentPage(string name, string uri)
		{
			InitializeComponent();
            MediaFileName = name;
            MediaUri = $"{uri}(format=m3u8-aapl)";

            BindingContext = this;
		}
	}
}