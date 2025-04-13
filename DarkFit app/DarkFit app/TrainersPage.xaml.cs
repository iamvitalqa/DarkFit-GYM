using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainersPage : ContentPage
    {
        public ObservableCollection<Trainers> Trainers{ get; set; }
        public TrainersPage()
        {
            InitializeComponent();

            Trainers = new ObservableCollection<Trainers>();
            BindingContext = this;
            LoadTrainers();

        }
        private async void LoadTrainers()
        {
            var trainersFromDb = await GetTrainersAsync();
            foreach (var trainer in trainersFromDb)
            {
                Trainers.Add(trainer);
            }
        }
        public async Task<List<Trainers>> GetTrainersAsync()
        {
            var trainers = new List<Trainers>();
            string connectionString = "Host=192.168.0.106;Port=5432;Database=DarkFit;Username=postgres;Password=admin;Timeout=15;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT worker_id, worker_surname, worker_name, worker_phone, worker_image FROM workers ORDER BY worker_surname";
                    using (var command = new NpgsqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.WriteLine($"Field {i}: {reader.GetName(i)}");
                        }
                        while (await reader.ReadAsync())
                        {
                            string workerIdStr = reader["worker_id"].ToString(); 
                            Console.WriteLine($"Worker ID: {workerIdStr}");
                            trainers.Add(new Trainers
                            {
                                Id = workerIdStr, 
                                Name = $"{reader["worker_surname"]} {reader["worker_name"]}",
                                Phone = reader["worker_phone"].ToString(),
                                Image = System.IO.Path.GetFileName(reader["worker_image"].ToString())
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось загрузить данные: {ex.Message}", "ОК");
                }
            }
            return trainers;
        }
        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.BindingContext is Trainers trainer)
            {
                string trainerId = trainer.Id;
                await Shell.Current.GoToAsync($"TrainerInfoPage?id={trainerId}");
            }
        }
    }
}