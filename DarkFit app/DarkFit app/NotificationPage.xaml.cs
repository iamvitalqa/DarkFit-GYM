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
                        n.message, 
                        n.created_at,
                        u.user_login AS sender_name
                    FROM notifications n
                    LEFT JOIN users u ON n.sender_user_id = u.user_id
                    WHERE n.user_id = @userId
                    ORDER BY n.created_at DESC";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("userId", _userId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Notifications.Add(new NotificationModel
                            {
                                Message = reader["message"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                SenderName = reader["sender_name"].ToString()
                            });
                        }
                    }
                }
            }
        }

        public class NotificationModel
        {
            public string Message { get; set; }
            public DateTime CreatedAt { get; set; }
            public string SenderName { get; set; }
        }
    }
}
