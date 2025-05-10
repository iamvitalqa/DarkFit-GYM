using DarkFit_app.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        public PaymentPage()
        {
            InitializeComponent();
        }
        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (decimal.TryParse(e.NewTextValue, out decimal enteredAmount) && enteredAmount > 0)
            {
                decimal finalAmount = enteredAmount;
                if (enteredAmount >= 20000)
                {
                    finalAmount += enteredAmount * 0.20m;  
                }
                else if (enteredAmount >= 15000)
                {
                    finalAmount += enteredAmount * 0.15m;  
                }
                else if (enteredAmount >= 10000)
                {
                    finalAmount += enteredAmount * 0.10m; 
                }
                costLabel.Text = $"Счёт будет пополнен на: {finalAmount:F2} ₽";
            }
            else
            {
                costLabel.Text = "Счёт будет пополнен на: 0 ₽";
            }
        }
        private void promoEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string promoCode = e.NewTextValue;
            if (promoCode == "NEWYEAR2025")
            {
                ApplyDiscount(0.30m); 
            }
            else
            {
                ApplyDiscount(0.00m); 
            }
        }
        private void ApplyDiscount(decimal discount)
        {
            UpdateRadioButtonText(radioButton1, 350, discount);
            UpdateRadioButtonText(radioButton2, 990, discount);
            UpdateRadioButtonText(radioButton3, 3490, discount);
            UpdateRadioButtonText(radioButton4, 14990, discount);
            UpdateRadioButtonText(radioButton5, 24990, discount);
        }
        private void UpdateRadioButtonText(RadioButton radioButton, decimal originalPrice, decimal discount)
        {
            decimal discountedPrice = originalPrice * (1 - discount);
            radioButton.Content = $"{originalPrice} ₽ (скидка {discount * 100}%): {discountedPrice:F2} ₽";
        }

        

        private async void depositButton_Clicked(object sender, EventArgs e)
        {
            if (decimal.TryParse(costLabel.Text.Replace("Счёт будет пополнен на: ", "").Replace(" ₽", ""), out decimal finalAmount) && finalAmount > 0)
            {
                await Navigation.PushAsync(new PayPage(finalAmount));
            }
            else
            {
                await DisplayAlert("Ошибка", "Введите корректную сумму пополнения.", "OK");
            }
        }

        private async void menuButton_Clicked(object sender, EventArgs e)
        {
            var popup = new MenuPopup();
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        private async void notificationButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NotificationPage));
        }
    }
}