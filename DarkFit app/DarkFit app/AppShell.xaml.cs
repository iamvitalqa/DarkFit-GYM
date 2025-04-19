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
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("TrainerInfoPage", typeof(TrainerInfoPage));
            Routing.RegisterRoute("BuyPage", typeof(BuyPage));
            Routing.RegisterRoute("AuthPage", typeof(AuthPage));
            Routing.RegisterRoute("PaymentPage", typeof(TrainersPage));
            Routing.RegisterRoute("MassageOrder", typeof(MassageOrder));
            
        }
    }
}