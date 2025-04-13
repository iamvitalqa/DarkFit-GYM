using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static DarkFit_app.AuthPage;
using System.Net.Http;
using Npgsql;
using Xamarin.Essentials;
namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        
        public RegisterPage()
        {
            InitializeComponent();
        }
        private void backButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AuthPage();
        }
        private async Task<bool> RegisterUser(string username, string password)
        {
            bool isRegistered = false;
            string connectionString = "Host=192.168.0.106;Port=5432;Database=DarkFit;Username=postgres;Password=admin;Timeout=5;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    var checkCommand = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE user_login = @username", connection);
                    checkCommand.Parameters.AddWithValue("@username", username);
                    int userExists = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());
                    if (userExists > 0)
                    {
                        await DisplayAlert("Ошибка регистрации!", "Пользователь с таким именем уже существует.", "Понял");
                    }
                    else
                    {
                        var insertCommand = new NpgsqlCommand("INSERT INTO users (user_login, user_password) VALUES (@username, @password)", connection);
                        insertCommand.Parameters.AddWithValue("@username", username);
                        insertCommand.Parameters.AddWithValue("@password", password);
                        int rowsAffected = await insertCommand.ExecuteNonQueryAsync();
                        isRegistered = rowsAffected > 0;
                        if (isRegistered)
                        {
                            await DisplayAlert("Успех!", "Пользователь успешно зарегистрирован.", "Отлично");
                        }
                        else
                        {
                            await DisplayAlert("Ошибка!", "Не удалось зарегистрировать пользователя.", "Понял");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка подключения!", $"Не удалось подключиться к базе данных: {ex.Message}", "Понял");
                }
            }
            return isRegistered;
        }
        private async void regButton_Clicked(object sender, EventArgs e)
        {
            string username = loginEntry.Text;
            string password = passwordEntry.Text;
            string confirmPassword = passwordRepeatEntry.Text;
            bool rememberMe = rememberMeSwitch.IsToggled;
            if (password != confirmPassword)
            {
                await DisplayAlert("Ошибка!", "Пароли не совпадают. Пожалуйста, проверьте ввод.", "Понял");
                return;
            }
            bool isValid = await RegisterUser(username, password);
            if (isValid)
            {
                if (rememberMe)
                {
                    Preferences.Set("IsLoggedIn", true);
                    Preferences.Set("Username", username);
                }
                Application.Current.MainPage = new AppShell(); // Переход на домашнюю страницу
            }
        }                    
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AuthPage();
        }
    }
}