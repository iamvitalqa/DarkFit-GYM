using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace DarkFit_app
{
    public partial class MassagePopup : PopupPage
    {
        public ObservableCollection<MassageGroupViewModel> MassageGroups { get; set; }

        public Command<Massage> SelectMassageCommand { get; set; }

        public event Action<Massage> MassageSelected;

        public MassagePopup()
        {
            InitializeComponent();
            MassageGroups = new ObservableCollection<MassageGroupViewModel>();
            BindingContext = this;

            SelectMassageCommand = new Command<Massage>(OnMassageSelected);

            LoadMassageData();
        }

        private void OnMassageSelected(Massage selectedMassage)
        {
            MassageSelected?.Invoke(selectedMassage);
            PopupNavigation.Instance.PopAsync(); // Закрываем popup
        }

        private async void LoadMassageData()
        {
            // Копируем логику из MassagePage — типы и услуги
            // Упрощённо здесь — можно вынести в общий сервис при необходимости
            var connection = new Npgsql.NpgsqlConnection(DarkFitDatabase.ConnectionString);
            await connection.OpenAsync();

            var typeCmd = new Npgsql.NpgsqlCommand("SELECT massage_type_id, massage_type_name FROM massages_type", connection);
            var reader = await typeCmd.ExecuteReaderAsync();
            var groups = new List<MassageGroupViewModel>();
            while (await reader.ReadAsync())
            {
                groups.Add(new MassageGroupViewModel
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    IsExpanded = true,
                    Massages = new ObservableCollection<Massage>(),
                    IsDataLoaded = false
                });
            }
            await reader.CloseAsync();

            foreach (var g in groups)
            {
                var massageCmd = new Npgsql.NpgsqlCommand("SELECT massageid, massagename, massagecost, massagedescription, massage_type_id FROM massages WHERE massage_type_id = @id", connection);
                massageCmd.Parameters.AddWithValue("@id", g.Id);
                var r = await massageCmd.ExecuteReaderAsync();
                while (await r.ReadAsync())
                {
                    g.Massages.Add(new Massage
                    {
                        Id = r.GetInt32(0),
                        Name = r.GetString(1),
                        Cost = r.GetDecimal(2),
                        Description = r.IsDBNull(3) ? "" : r.GetString(3),
                        TypeId = r.GetInt32(4)
                    });
                }
                await r.CloseAsync();
            }

            connection.Close();

            Device.BeginInvokeOnMainThread(() =>
            {
                MassageGroups.Clear();
                foreach (var g in groups)
                    MassageGroups.Add(g);
            });
        }
    }
}
