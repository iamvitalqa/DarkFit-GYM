using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DarkFit_app
{
    public partial class App : Application
    {
        public static int CurrentUserId { get; set; } = -1;

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

                if (isLoggedIn)
                {
                    // Устанавливаем сохранённый UserId при запуске
                    int savedUserId = Preferences.Get("UserId", -1);
                    if (savedUserId != -1)
                    {
                        CurrentUserId = savedUserId;
                    }
                }

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    if (isLoggedIn)
                    {
                        // Переход на нужную вкладку
                        await Shell.Current.GoToAsync("//PaymentPage");
                    }
                    else
                    {
                        // Если не залогинен — переходим на страницу авторизации
                        Application.Current.MainPage = new AuthPage();
                    }
                });
            });
        }

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }
    }
}
