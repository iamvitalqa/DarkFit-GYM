using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Npgsql;

namespace DarkFit_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackPopup : PopupPage
    {
        private readonly int _userId;
        private int? _clientId;

        public FeedbackPopup(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadClientDataAsync(); // загружаем clientId и имя
        }

        private async void LoadClientDataAsync()
        {
            try
            {
                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT client_id, clientsurname, clientname, clientpatronymic FROM clients WHERE user_id = @userId";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("userId", _userId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                _clientId = Convert.ToInt32(reader["client_id"]);

                                string fullName = $"{reader["clientsurname"]} {reader["clientname"]} {reader["clientpatronymic"]}";
                                NameEntry.Text = fullName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить данные клиента: {ex.Message}", "ОК");
            }
        }

        private async Task SendFeedbackAsync(string message)
        {
            if (_clientId == null)
            {
                await DisplayAlert("Ошибка", "Не удалось определить клиента для отправки сообщения.", "ОК");
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();

                    string insertQuery = @"
                        INSERT INTO feedback (comment, worker_id, client_id, created_at) 
                        VALUES (@comment, 1, @client_id, @created_at);
                    ";

                    using (var cmd = new NpgsqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("comment", message);
                        cmd.Parameters.AddWithValue("client_id", _clientId.Value);
                        cmd.Parameters.AddWithValue("created_at", DateTime.UtcNow);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    await DisplayAlert("Успешно", "Ваше обращение отправлено!", "ОК");
                    await PopupNavigation.Instance.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Произошла ошибка при отправке: {ex.Message}", "ОК");
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
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            var message = MessageEditor.Text?.Trim();

            if (string.IsNullOrWhiteSpace(message))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите сообщение.", "ОК");
                return;
            }

            await SendFeedbackAsync(message);
        }
    }
}
