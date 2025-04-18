using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MassagePage : ContentPage, INotifyPropertyChanged
    {
        private readonly string connectionString = DarkFitDatabase.ConnectionString;

        public ObservableCollection<MassageGroupViewModel> MassageGroups { get; set; }

        public MassagePage()
        {
            InitializeComponent();
            MassageGroups = new ObservableCollection<MassageGroupViewModel>();
            BindingContext = this;
            LoadMassageData();
        }

        private async void LoadMassageData()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Загружаем типы массажа
                    var typeQuery = "SELECT massage_type_id, massage_type_name FROM massages_type";
                    var typeCommand = new NpgsqlCommand(typeQuery, connection);
                    var typeReader = await typeCommand.ExecuteReaderAsync();
                    var types = new List<MassageGroupViewModel>();

                    while (await typeReader.ReadAsync())
                    {
                        types.Add(new MassageGroupViewModel
                        {
                            Id = typeReader.GetInt32(0),
                            Name = typeReader.GetString(1),
                            IsExpanded = false,
                            Massages = new ObservableCollection<Massage>(),
                            IsDataLoaded = false // Изначально данные не загружены
                        });
                    }
                    await typeReader.CloseAsync();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MassageGroups.Clear();
                        foreach (var g in types)
                            MassageGroups.Add(g);
                    });
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", ex.Message, "OK");
                }
            }
        }

        private async void ToggleExpandTapped(object sender, EventArgs e)
        {
            if (sender is StackLayout layout && layout.BindingContext is MassageGroupViewModel group)
            {
                if (group.IsExpanded)
                {
                    // Если категория уже раскрыта, просто меняем состояние
                    group.IsExpanded = false;
                }
                else
                {
                    // Если категория раскрыта впервые, загружаем услуги
                    group.IsExpanded = true;

                    // Загружаем услуги для группы, если они еще не были загружены
                    if (!group.IsDataLoaded)
                    {
                        await LoadMassagesForGroup(group);
                        group.IsDataLoaded = true; // Помечаем, что данные загружены
                    }
                }
            }
        }

        private async Task LoadMassagesForGroup(MassageGroupViewModel group)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Загружаем услуги массажа для выбранной категории
                    var massageQuery = "SELECT massageid, massagename, massagecost, massagedescription, massage_type_id FROM massages WHERE massage_type_id = @typeId";
                    var massageCommand = new NpgsqlCommand(massageQuery, connection);
                    massageCommand.Parameters.AddWithValue("@typeId", group.Id);
                    var massageReader = await massageCommand.ExecuteReaderAsync();

                    while (await massageReader.ReadAsync())
                    {
                        var massage = new Massage
                        {
                            Id = massageReader.GetInt32(0),
                            Name = massageReader.GetString(1),
                            Cost = massageReader.GetDecimal(2),
                            Description = massageReader.GetString(3),
                            TypeId = massageReader.GetInt32(4)
                        };

                        group.Massages.Add(massage);  // Добавляем в коллекцию текущей группы
                    }
                    await massageReader.CloseAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", ex.Message, "OK");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class MassageGroupViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Massage> Massages { get; set; }

        // Флаг для проверки, были ли загружены данные
        public bool IsDataLoaded { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Massage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
    }
}
