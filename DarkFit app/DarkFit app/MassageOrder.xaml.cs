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
        public MassageOrder()
        {
            InitializeComponent();
        }

        private async void createorderButton_Clicked(object sender, EventArgs e)
        {
            if (applyCheckBox.IsChecked)
            {
                await DisplayAlert("Запись", "Вы успешно записаны!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Ошибка", "Пожалуйста, подтвердите отсутствие противопоказаний.", "OK");
            }
        }

        private async void chooseorderButton_Clicked(object sender, EventArgs e)
        {
            var popup = new MassagePopup();
            popup.MassageSelected += (selectedMassage) =>
            {
                // Сюда сохраняй выбранный массаж
                chooseorderButton.Text = $"Вы выбрали: {selectedMassage.Name}";
                // Можно сохранить в поле, если потом нужна ID/стоимость и т.п.
            };

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }
    }
}
