using Npgsql;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(TrainerId), "id")]
    public partial class TrainerInfoPage : ContentPage
    {
        private string _trainerId;
        public string TrainerId
        {
            get => _trainerId;
            set
            {
                _trainerId = value;
                LoadTrainerInfo(_trainerId); // Загружаем информацию при установке значения
            }
        }
        public TrainerInfoPage()
        {
            InitializeComponent();
        }
        private async Task LoadTrainerInfo(string id)
        {
            string connectionString = DarkFitDatabase.ConnectionString;
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT worker_id, worker_surname, worker_name, worker_patronymic,worker_pricelist, worker_phone, worker_image, worker_description FROM workers WHERE worker_id = @id";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var trainer = new Trainers
                                {
                                    Id = reader["worker_id"].ToString(),
                                    Name = $"{reader["worker_surname"]} {reader["worker_name"]} {reader["worker_patronymic"]} ", 
                                    Phone = reader["worker_phone"].ToString(),
                                    PriceList = reader["worker_pricelist"].ToString() ,
                                    Image = System.IO.Path.GetFileName(reader["worker_image"].ToString()),
                                    Description = reader["worker_description"].ToString()
                                };
                                BindingContext = trainer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка загрузки данных: {ex.Message}", "OK");
            }
        }

        private void whatsappTrainerButton_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is Trainers trainer && !string.IsNullOrWhiteSpace(trainer.Phone))
            {
                try
                {
                    string phoneNumber = trainer.Phone.Replace("+", "").Replace(" ", ""); // Убираем пробелы и "+"
                    string url = $"https://wa.me/{phoneNumber}";

                    Launcher.OpenAsync(new Uri(url));
                }
                catch (Exception ex)
                {
                    DisplayAlert("Ошибка", $"Не удалось открыть WhatsApp: {ex.Message}", "OK");
                }
            }
        }

        private void callTrainerButton_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is Trainers trainer && !string.IsNullOrWhiteSpace(trainer.Phone))
            {
                try
                {
                    PhoneDialer.Open(trainer.Phone);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Ошибка", $"Не удалось открыть приложение Телефон: {ex.Message}", "OK");
                }
            }
        }

        private async void callBackTrainerButton_Clicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert("Запись", "Вы хотите чтобы тренер Вам перезвонил?", "Да", "Нет");
            if (!confirmed) return;

            try
            {
                int currentUserId = Preferences.Get("UserId", -1);
                if (currentUserId == -1)
                {
                    await DisplayAlert("Ошибка", "Вы не авторизованы", "OK");
                    return;
                }

                string connectionString = DarkFitDatabase.ConnectionString;

                string clientName = "";
                string message = "";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Получаем ФИО клиента
                    var getClientCommand = new NpgsqlCommand(
                        "SELECT clientsurname, clientname, clientpatronymic FROM clients WHERE user_id = @userId", connection);
                    getClientCommand.Parameters.AddWithValue("@userId", currentUserId);
                    using (var reader = await getClientCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            clientName = $"{reader["clientsurname"]} {reader["clientname"]} {reader["clientpatronymic"]}";
                        }
                    }

                    // Формируем сообщение
                    message = $"{clientName}, хочет на вашу тренировку!";
                }

                // Вставляем уведомление для тренера (user_id из workers)
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var getTrainerUserIdCmd = new NpgsqlCommand("SELECT user_id FROM workers WHERE worker_id = @workerId", connection);
                    getTrainerUserIdCmd.Parameters.AddWithValue("@workerId", TrainerId);

                    var trainerUserIdObj = await getTrainerUserIdCmd.ExecuteScalarAsync();

                    if (trainerUserIdObj != null && int.TryParse(trainerUserIdObj.ToString(), out int trainerUserId))
                    {
                        var insertCommand = new NpgsqlCommand(
                            "INSERT INTO notifications (user_id, message, created_at, is_read) VALUES (@userId, @message, NOW(), false)", connection);
                        insertCommand.Parameters.AddWithValue("@userId", trainerUserId);
                        insertCommand.Parameters.AddWithValue("@message", message);
                        await insertCommand.ExecuteNonQueryAsync();
                        await DisplayAlert("Уведомление", "Ожидайте звонка!", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Не удалось определить тренера", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Произошла ошибка: {ex.Message}", "OK");
            }
        }
    }
}
