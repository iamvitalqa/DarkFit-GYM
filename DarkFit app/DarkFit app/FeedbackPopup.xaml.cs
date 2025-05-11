using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Npgsql;

namespace DarkFit_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackPopup : PopupPage
    {
        private readonly int _userId;

        public FeedbackPopup(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadUserFullNameAsync();
        }

        private async void LoadUserFullNameAsync()
        {
            try
            {
                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();

                    // Попытка получить ФИО из clients
                    string clientQuery = @"
                        SELECT clientsurname, clientname, clientpatronymic 
                        FROM clients 
                        WHERE user_id = @userId";

                    using (var cmd = new NpgsqlCommand(clientQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("userId", _userId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                NameEntry.Text = $"{reader["clientsurname"]} {reader["clientname"]} {reader["clientpatronymic"]}";
                                return;
                            }
                        }
                    }

                    // Если не найден client — пробуем worker
                    string workerQuery = @"
                        SELECT worker_surname, worker_name, worker_patronymic 
                        FROM workers 
                        WHERE user_id = @userId";

                    using (var cmd = new NpgsqlCommand(workerQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("userId", _userId);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                NameEntry.Text = $"{reader["worker_surname"]} {reader["worker_name"]} {reader["worker_patronymic"]}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить имя пользователя: {ex.Message}", "ОК");
            }
        }

        private async Task SendNotificationToAdminsAsync(string message)
        {
            try
            {
                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();

                    // Получаем всех пользователей с ролью admin (role_id = 1)
                    string getAdminsQuery = @"
                        SELECT user_id 
                        FROM users 
                        WHERE role_id = 1";

                    using (var getAdminsCmd = new NpgsqlCommand(getAdminsQuery, conn))
                    using (var reader = await getAdminsCmd.ExecuteReaderAsync())
                    {
                        var adminUserIds = new System.Collections.Generic.List<int>();
                        while (await reader.ReadAsync())
                        {
                            adminUserIds.Add(reader.GetInt32(0));
                        }

                        reader.Close(); // Закрыть, прежде чем выполнять другие команды

                        foreach (var adminId in adminUserIds)
                        {
                            string insertQuery = @"
                                INSERT INTO notifications (sender_user_id, user_id, message, created_at, is_read)
                                VALUES (@sender_user_id, @receiver_user_id, @message, @created_at, false)";

                            using (var insertCmd = new NpgsqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("sender_user_id", _userId);
                                insertCmd.Parameters.AddWithValue("receiver_user_id", adminId);
                                insertCmd.Parameters.AddWithValue("message", message);
                                insertCmd.Parameters.AddWithValue("created_at", DateTime.UtcNow);

                                await insertCmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }

                await DisplayAlert("Успешно", "Ваше обращение отправлено администрации!", "ОК");
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Ошибка при отправке: {ex.Message}", "ОК");
            }
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void OnAnonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            NameEntry.IsEnabled = !e.Value;
            if (e.Value)
            {
                NameEntry.Text = string.Empty;
            }
            else
            {
                LoadUserFullNameAsync();
            }
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            var message = MessageEditor.Text?.Trim();

            if (string.IsNullOrWhiteSpace(message))
            {
                await DisplayAlert("Ошибка", "Введите сообщение.", "ОК");
                return;
            }

            await SendNotificationToAdminsAsync(message);
        }
    }
}
