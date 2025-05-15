using DarkFit_app.Views;
using Npgsql;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        public PaymentPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                int userId = App.CurrentUserId;
                var userInfo = await GetUserInfoAsync(userId);

                if (userInfo != null)
                {
                    FullNameLabel.Text = userInfo.Item1;
                    RoleLabel.Text = userInfo.Item2;
                }
                else
                {
                    FullNameLabel.Text = "Неизвестный пользователь";
                    RoleLabel.Text = "";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Ошибка загрузки профиля: {ex.Message}", "OK");
            }
        }

        private async Task<Tuple<string, string>> GetUserInfoAsync(int userId)
        {
            var connection = new NpgsqlConnection(DarkFitDatabase.ConnectionString);
            try
            {
                await connection.OpenAsync();

                string query = @"
            SELECT u.role_id, r.role_name,
                   c.clientsurname, c.clientname, c.clientpatronymic,
                   w.worker_surname, w.worker_name, w.worker_patronymic
            FROM users u
            LEFT JOIN roles r ON u.role_id = r.role_id
            LEFT JOIN clients c ON c.user_id = u.user_id
            LEFT JOIN workers w ON w.user_id = u.user_id
            WHERE u.user_id = @userId;
        ";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("userId", userId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string roleCode = reader["role_name"]?.ToString();
                            string role = "Неизвестно";

                            if (roleCode == "client")
                                role = "Роль: Клиент";
                            else if (roleCode == "worker")
                                role = "Роль: Тренер";
                            else if (roleCode == "admin")
                                role = "Роль: Руководитель";

                            string fullName = "";

                            if (reader["clientsurname"] != DBNull.Value)
                            {
                                fullName = $"{reader["clientsurname"]} {reader["clientname"]} {reader["clientpatronymic"]}";
                            }
                            else if (reader["worker_surname"] != DBNull.Value)
                            {
                                fullName = $"{reader["worker_surname"]} {reader["worker_name"]} {reader["worker_patronymic"]}";
                            }
                            else
                            {
                                fullName = "ФИО не найдено";
                            }

                            return Tuple.Create(fullName, role);
                        }
                    }
                }

                return null;
            }
            finally
            {
                connection.Dispose();
            }
        }




        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (decimal.TryParse(e.NewTextValue, out decimal enteredAmount) && enteredAmount > 0)
            {
                decimal finalAmount = enteredAmount;
                if (enteredAmount >= 20000)
                    finalAmount += enteredAmount * 0.20m;
                else if (enteredAmount >= 15000)
                    finalAmount += enteredAmount * 0.15m;
                else if (enteredAmount >= 10000)
                    finalAmount += enteredAmount * 0.10m;

                costLabel.Text = $"Счёт будет пополнен на: {finalAmount:F2} ₽";
            }
            else
            {
                costLabel.Text = "Счёт будет пополнен на: 0 ₽";
            }
        }

        private void promoEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string promoCode = e.NewTextValue;
            if (promoCode == "NEWYEAR2025")
                ApplyDiscount(0.30m);
            else
                ApplyDiscount(0.00m);
        }

        private void ApplyDiscount(decimal discount)
        {
            UpdateRadioButtonText(radioButton1, 350, discount);
            UpdateRadioButtonText(radioButton2, 990, discount);
            UpdateRadioButtonText(radioButton3, 3490, discount);
            UpdateRadioButtonText(radioButton4, 14990, discount);
            UpdateRadioButtonText(radioButton5, 24990, discount);
        }

        private void UpdateRadioButtonText(RadioButton radioButton, decimal originalPrice, decimal discount)
        {
            decimal discountedPrice = originalPrice * (1 - discount);
            radioButton.Content = $"{originalPrice} ₽ (скидка {discount * 100}%): {discountedPrice:F2} ₽";
        }

        private async void depositButton_Clicked(object sender, EventArgs e)
        {
            var rawText = costLabel.Text.Replace("Счёт будет пополнен на: ", "").Replace(" ₽", "").Replace(",", ".");
            if (decimal.TryParse(rawText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal finalAmount) && finalAmount > 0)
            {
                await Navigation.PushAsync(new PayPage(finalAmount));
            }
            else
            {
                await DisplayAlert("Ошибка", "Введите корректную сумму пополнения.", "OK");
            }
        }

        private async void menuButton_Clicked(object sender, EventArgs e)
        {
            var popup = new MenuPopup();
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        private async void notificationButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NotificationPage));
        }
    }
}
