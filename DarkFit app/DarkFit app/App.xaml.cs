using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DarkFit_app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Устанавливаем начальную страницу на AppShell
            MainPage = new AppShell();

            // Регистрация маршрутов, если это нужно
            Routing.RegisterRoute("TrainerInfoPage", typeof(TrainerInfoPage));

            Task.Run(async () =>
            {
                bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (isLoggedIn)
                    {
                        // Переход на TrainersPage внутри Shell
                        await Shell.Current.GoToAsync("//PaymentPage");
                    }
                    else
                    {
                        // Переход на AuthPage внутри Shell
                        Application.Current.MainPage = new AuthPage();
                    }
                });
            });

        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
