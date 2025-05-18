using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace DarkFit_app
{
    public partial class MassageOrder : ContentPage
    {
        private Massage selectedMassage;


        public MassageOrder()
        {
            InitializeComponent();
        }


        private async Task SendNotificationToAdminsAsync(string message)
        {
            using (var conn = new Npgsql.NpgsqlConnection(DarkFitDatabase.ConnectionString))
            {
                await conn.OpenAsync();

                // Получаем всех админов (role_id = 1)
                var cmd = new Npgsql.NpgsqlCommand("SELECT user_id FROM users WHERE role_id = 1", conn);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var adminIds = new List<int>();
                    while (await reader.ReadAsync())
                    {
                        adminIds.Add(reader.GetInt32(0));
                    }
                    reader.Close();

                    foreach (var adminId in adminIds)
                    {
                        var insertCmd = new Npgsql.NpgsqlCommand(
                            @"INSERT INTO notifications (user_id, sender_user_id, message, created_at, is_read)
                      VALUES (@userId, @senderUserId, @message, NOW(), false)", conn);

                        insertCmd.Parameters.AddWithValue("@userId", adminId);
                        insertCmd.Parameters.AddWithValue("@senderUserId", App.CurrentUserId); // предполагаем, что CurrentUserId уже установлен
                        insertCmd.Parameters.AddWithValue("@message", message);

                        await insertCmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }




        private async void createorderButton_Clicked(object sender, EventArgs e)
        {
            if (!applyCheckBox.IsChecked)
            {
                await DisplayAlert("Ошибка", "Пожалуйста, подтвердите отсутствие противопоказаний.", "OK");
                return;
            }

            if (selectedMassage == null)
            {
                await DisplayAlert("Ошибка", "Пожалуйста, выберите тип массажа.", "OK");
                return;
            }

            // Отправка уведомления админу
            string message = $"Хочу записаться на массаж: {selectedMassage.Name}";
            await SendNotificationToAdminsAsync(message);

            await DisplayAlert("Заявка", "Заявка успешно создана!", "OK");
            await Shell.Current.GoToAsync("..");
        }


        private async void chooseorderButton_Clicked(object sender, EventArgs e)
        {
            var popup = new MassagePopup();
            popup.MassageSelected += (selected) =>
            {
                selectedMassage = selected;
                chooseorderButton.Text = $"Вы выбрали: {selected.Name}";
            };

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

    }
}
