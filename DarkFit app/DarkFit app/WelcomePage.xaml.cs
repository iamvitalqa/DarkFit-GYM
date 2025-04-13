using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace DarkFit_app
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
			InitializeComponent ();
			//StartTimer ();
		}
        private async void StartTimer()
        {
            await Task.Delay(2000);
            Application.Current.MainPage = new AuthPage();
        }
    }
}