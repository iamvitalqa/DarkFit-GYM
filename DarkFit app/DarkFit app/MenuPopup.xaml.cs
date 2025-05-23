﻿using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Npgsql;


namespace DarkFit_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPopup : PopupPage
    {
        public string BalanceText { get; set; } = "💰 Баланс: загрузка...";
        public MenuPopup()
        {
            InitializeComponent();
            BindingContext = this;
            LoadBalanceAsync();
        }

        private async void LoadBalanceAsync()
        {
            try
            {
                int userId = App.CurrentUserId;

                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();

                    var cmd = new NpgsqlCommand(@"
                        SELECT clientbalance 
                        FROM clients 
                        WHERE user_id = @userId", conn);

                    cmd.Parameters.AddWithValue("userId", userId);

                    var result = await cmd.ExecuteScalarAsync();

                    if (result != null && decimal.TryParse(result.ToString(), out decimal balance))
                    {
                        BalanceText = $"💰 Баланс: {balance:0.##}₽";
                    }
                    else
                    {
                        BalanceText = "💰 Баланс: 0₽";
                    }

                    OnPropertyChanged(nameof(BalanceText)); // Обновление UI
                }
            }
            catch
            {
                BalanceText = "💰 Баланс: ошибка";
                OnPropertyChanged(nameof(BalanceText));
            }
        }
        private async void logoutButton_Clicked(object sender, EventArgs e)
        {
            bool confirmLogout = await Application.Current.MainPage.DisplayAlert("Выход", "Вы уверены, что хотите выйти из аккаунта?", "Да", "Нет");
            if (confirmLogout)
            {
                Preferences.Set("IsLoggedIn", false);
                Preferences.Remove("Username");

                // Закрываем popup перед переходом
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAllAsync();

                // Переход к AuthPage
                Application.Current.MainPage = new AuthPage();
            }
        }
        private async void feedbackButton_CLicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();

            int userId = Preferences.Get("UserId", -1);
            if (userId != -1)
            {
                await PopupNavigation.Instance.PushAsync(new FeedbackPopup(userId));
            }
            else
            {
                await DisplayAlert("Ошибка", "Вы не авторизованы.", "OK");
            }

        }
        private async void settingsButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            await Shell.Current.GoToAsync(nameof(SettingsPage));
            

        }
    }
}
