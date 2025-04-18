using System;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Npgsql;
using Xamarin.Essentials;

namespace DarkFit_app
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AuthPage : ContentPage
	{
		public AuthPage()
		{
			InitializeComponent();
		}
        //private async Task<bool> CheckLogin(string username, string password)
        //{
        //    bool isValid = false;
        //    string connectionString = DarkFitDatabase.ConnectionString;
        //    using (var connection = new NpgsqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            await connection.OpenAsync();
        //            var command = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE user_login = @username AND user_password = @password", connection);
        //            command.Parameters.AddWithValue("@username", username);
        //            command.Parameters.AddWithValue("@password", password);
        //            int count = Convert.ToInt32(await command.ExecuteScalarAsync());
        //            isValid = count > 0;
        //        }
        //        catch (Exception ex)
        //        {
        //            await DisplayAlert("Ошибка подключения!", $"Не удалось подключиться к базе данных: {ex.Message}", "Понял");
        //        }
        //    }
        //    return isValid;
        //}






        private async Task<int?> GetUserId(string username, string password)
        {
            string connectionString = DarkFitDatabase.ConnectionString;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var command = new NpgsqlCommand("SELECT user_id FROM users WHERE user_login = @username AND user_password = @password", connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    var result = await command.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка подключения!", $"Не удалось подключиться к базе данных: {ex.Message}", "Понял");
                }
            }
            return null;
        }



        private async void loginButton_Clicked(object sender, EventArgs e)
        {
            string username = loginEntry.Text;
            string password = passwordEntry.Text;
            bool rememberMe = rememberMeSwitch.IsToggled;

            int? userId = await GetUserId(username, password);

            if (userId.HasValue)
            {
                // сохраняем в Preferences
                Preferences.Set("UserId", userId.Value);

                if (rememberMe)
                {
                    Preferences.Set("IsLoggedIn", true);
                    Preferences.Set("Username", username);
                    Preferences.Set("UserId", userId.Value);
                }

                await DisplayAlert("Успех!", "Вы успешно вошли.", "OK");

                Application.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync("//PaymentPage");
            }
            else
            {
                await DisplayAlert("Ошибка!", "Неверное имя пользователя или пароль.", "Понял");
            }
        }
        private void registerButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegisterPage();
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegisterPage();
        }
    }
}