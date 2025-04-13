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

            // Устанавливаем AppShell как главную страницу
            MainPage = new AppShell();

            // Регистрируем маршруты
            Routing.RegisterRoute("TrainerInfoPage", typeof(TrainerInfoPage));

            Task.Run(async () =>
            {
                bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    if (isLoggedIn)
                    {
                        // Убедимся, что Shell полностью загружен перед навигацией
                        //await Task.Delay(100); // небольшая задержка помогает в некоторых случаях

                        // Навигация к нужной вкладке
                        await Shell.Current.GoToAsync("//PaymentPage");
                    }
                    else
                    {
                        // Перенаправление на AuthPage
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
