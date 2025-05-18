using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Npgsql;
using Xamarin.Essentials;

namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        public ObservableCollection<NotificationModel> Notifications { get; set; }

        private int _userId;
        private int _roleId;

        public NotificationPage()
        {
            InitializeComponent();
            Notifications = new ObservableCollection<NotificationModel>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _userId = Preferences.Get("UserId", -1);
            if (_userId == -1)
            {
                await DisplayAlert("Ошибка", "Пользователь не авторизован", "ОК");
                return;
            }

            using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT role_id FROM users WHERE user_id = @userId", conn);
                cmd.Parameters.AddWithValue("userId", _userId);
                var result = await cmd.ExecuteScalarAsync();

                if (result == null || !int.TryParse(result.ToString(), out _roleId))
                {
                    await DisplayAlert("Ошибка", "Не удалось определить роль пользователя", "ОК");
                    return;
                }
            }

            await LoadNotifications();
        }

        private async System.Threading.Tasks.Task LoadNotifications()
        {
            Notifications.Clear();
            using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
            {
                await conn.OpenAsync();

                string query = @"
                    SELECT 
                        n.notification_id,
                        n.message, 
                        n.created_at,
                        n.sender_user_id,
                        n.is_read,
                        u.role_id,
                        c.clientsurname, c.clientname,
                        w.worker_surname, w.worker_name
                    FROM notifications n
                    LEFT JOIN users u ON n.sender_user_id = u.user_id
                    LEFT JOIN clients c ON u.role_id = 3 AND c.user_id = u.user_id
                    LEFT JOIN workers w ON u.role_id = 2 AND w.user_id = u.user_id
                    WHERE n.user_id = @userId
                    ORDER BY n.created_at DESC";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("userId", _userId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string senderName = string.Empty;

                            if (!(reader["sender_user_id"] is DBNull))
                            {
                                int senderRole = reader["role_id"] is DBNull ? 0 : Convert.ToInt32(reader["role_id"]);

                                if (senderRole == 3)
                                {
                                    senderName = $"{reader["clientsurname"]} {reader["clientname"]}";
                                }
                                else if (senderRole == 2)
                                {
                                    senderName = $"{reader["worker_surname"]} {reader["worker_name"]}";
                                }
                                else if (senderRole == 1)
                                {
                                    senderName = "Руководство DarkFit";
                                }
                            }

                            Notifications.Add(new NotificationModel
                            {
                                NotificationId = reader["notification_id"] is DBNull ? 0 : Convert.ToInt32(reader["notification_id"]),
                                Message = reader["message"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                SenderUserId = reader["sender_user_id"] is DBNull ? 0 : Convert.ToInt32(reader["sender_user_id"]),
                                SenderName = senderName,
                                IsRead = reader["is_read"] is DBNull ? false : Convert.ToBoolean(reader["is_read"])
                            });
                        }
                    }
                }
            }
        }

        private async void OnNotificationTapped(object sender, ItemTappedEventArgs e)
        {
            var notification = e.Item as NotificationModel;
            if (notification == null || notification.IsRead)
                return;

            string messageToSend = string.Empty;

            if (_roleId == 2) // Тренер
            {
                bool confirm = await DisplayAlert("Ответ", $"Отправить клиенту ответ: \"Я скоро с Вами свяжусь! 😉\"?", "Да", "Нет");
                if (!confirm) return;
                messageToSend = "Я скоро с Вами свяжусь! 😉";
            }
            else if (_roleId == 1) // Админ
            {
                if (notification.Message.Contains("Хочу записаться на массаж:"))
                {
                    
                    bool confirm = await DisplayAlert("Ответ", $"Отправить клиенту: \"Ваша заявка на массаж принята. Мы с Вами свяжемся! 🤗\"", "Да", "Нет");
                    if (!confirm) return;

                    messageToSend = $"Ваша заявка на массаж принята. Мы с Вами свяжемся! 🤗";
                }
                else
                {
                    bool confirm = await DisplayAlert("Ответ", $"Отправить клиенту: \"Ваш заказ готов и ожидает получения в баре\"?", "Да", "Нет");
                    if (!confirm) return;

                    messageToSend = "Ваш заказ готов и ожидает получения в баре! 😎";
                }
            }
            else
            {
                return; // Остальные роли не отвечают
            }

            try
            {
                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();

                    // Отправляем новое уведомление
                    var insertCommand = new NpgsqlCommand(
                        @"INSERT INTO notifications (user_id, sender_user_id, message, created_at, is_read)
                  VALUES (@userId, @senderUserId, @message, NOW(), false)", conn);

                    insertCommand.Parameters.AddWithValue("@userId", notification.SenderUserId);
                    insertCommand.Parameters.AddWithValue("@senderUserId", _userId);
                    insertCommand.Parameters.AddWithValue("@message", messageToSend);

                    await insertCommand.ExecuteNonQueryAsync();

                    // Обновляем флаг is_read для текущего уведомления
                    var updateCommand = new NpgsqlCommand(
                        "UPDATE notifications SET is_read = TRUE WHERE notification_id = @notificationId", conn);
                    updateCommand.Parameters.AddWithValue("@notificationId", notification.NotificationId);
                    await updateCommand.ExecuteNonQueryAsync();
                }

                // Обновляем локально
                notification.IsRead = true;
                NotificationListView.ItemsSource = null;
                NotificationListView.ItemsSource = Notifications;

                await DisplayAlert("Отправлено", "Ответ отправлен клиенту!", "ОК");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Ошибка при отправке ответа: {ex.Message}", "ОК");
            }
        }

       
        public class NotificationModel
        {
            public int NotificationId { get; set; }
            public string Message { get; set; }
            public DateTime CreatedAt { get; set; }
            public string SenderName { get; set; }
            public int SenderUserId { get; set; }
            public bool IsRead { get; set; }
        }
    }
}
