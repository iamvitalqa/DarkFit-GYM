using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayPage : ContentPage
    {
        public PayPage(decimal totalCost)
        {
            InitializeComponent();

            // Форматируем ссылку с учётом суммы
            string formattedAmount = totalCost.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string paymentUrl = $"https://yoomoney.ru/to/4100116757351568?amount={formattedAmount}";

            Console.WriteLine("Сформированная ссылка: " + paymentUrl);

            // Устанавливаем источник WebView
            PaymentWebView.Source = paymentUrl;

            // Обработка переходов
            PaymentWebView.Navigating += PaymentWebView_Navigating;
        }

        private async void PaymentWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.Contains("success"))
            {
                await DisplayAlert("Оплата успешна", "Ваш платеж прошел успешно!", "OK");
                await Navigation.PopAsync();
            }
            else if (e.Url.Contains("error"))
            {
                await DisplayAlert("Ошибка оплаты", "Что-то пошло не так. Попробуйте ещё раз.", "OK");
                e.Cancel = true;
            }
        }
    }
}
