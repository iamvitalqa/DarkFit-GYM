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
        private decimal discountedAmount = 0; // будет применяться в OnConfirmCardPayment

        public PaymentPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadUserInfoAsync();
            await LoadCardExpiryAsync(); // загрузка даты действия карты
        }

        private async Task LoadUserInfoAsync()
        {
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

        private async void OnConfirmCardPayment(object sender, EventArgs e)
        {
            int durationDays = 0;
            decimal amount = 0;
            string description = "";

            if (radioButton1.IsChecked) { amount = 350; durationDays = 1; description = "1 день"; }
            else if (radioButton2.IsChecked) { amount = 990; durationDays = 7; description = "7 дней"; }
            else if (radioButton3.IsChecked) { amount = 3490; durationDays = 31; description = "1 месяц"; }
            else if (radioButton4.IsChecked) { amount = 14990; durationDays = 180; description = "6 месяцев"; }
            else if (radioButton5.IsChecked) { amount = 24990; durationDays = 365; description = "1 год"; }

            if (amount == 0 || durationDays == 0)
            {
                await DisplayAlert("Ошибка", "Выберите тип абонемента", "OK");
                return;
            }

            bool isConfirmed = await DisplayAlert(
                "Подтверждение оплаты",
                $"Вы уверены, что хотите приобрести абонемент на {description} за {amount} ₽?",
                "Да", "Нет");

            if (!isConfirmed)
                return;

            int userId = App.CurrentUserId;
            bool success = await ProcessCardPurchaseAsync(userId, amount, durationDays);

            if (success)
            {
                await LoadCardExpiryAsync();
                await DisplayAlert("Успешно", "Абонемент успешно оформлен", "ОК");
            }
            else
            {
                await DisplayAlert("Ошибка", "Недостаточно средств или произошла ошибка", "ОК");
            }
        }


        private async Task<bool> ProcessCardPurchaseAsync(int userId, decimal price, int days)
        {
            using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
            {
                await conn.OpenAsync();

                var cmd = new NpgsqlCommand(@"
                    UPDATE clients
                    SET 
                        clientbalance = clientbalance - @price,
                        card_expiry = 
                            CASE 
                                WHEN card_expiry IS NULL OR card_expiry < CURRENT_DATE THEN CURRENT_DATE + @days
                                ELSE card_expiry + @days 
                            END
                    WHERE user_id = @userId AND clientbalance >= @price
                    RETURNING card_expiry;
                ", conn);

                cmd.Parameters.AddWithValue("price", price);
                cmd.Parameters.AddWithValue("days", days);
                cmd.Parameters.AddWithValue("userId", userId);

                var result = await cmd.ExecuteScalarAsync();
                return result != null;
            }
        }

        private async Task LoadCardExpiryAsync()
        {
            try
            {
                int userId = App.CurrentUserId;
                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new NpgsqlCommand("SELECT card_expiry FROM clients WHERE user_id = @userId", conn);
                    cmd.Parameters.AddWithValue("userId", userId);

                    var result = await cmd.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        DateTime expiry = Convert.ToDateTime(result);
                        cardExpirationLabel.Text = $"Срок действия карты истекает {expiry:dd.MM.yyyy}";
                        cardExpirationLabel.IsVisible = true;
                    }
                    else
                    {
                        cardExpirationLabel.IsVisible = false;
                    }
                }
            }
            catch
            {
                cardExpirationLabel.IsVisible = false;
            }
        }
    }
}
