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
            string paymentUrl = $"https://yoomoney.ru/quickpay/shop-widget?writer=seller&targets=Оплата услуги&targets-hint=&default-sum={formattedAmount}&sum-editable=false&button-text=11&payment-type-choice=on&mobile-payment-type-choice=on&successURL=https://example.com/success&account=4100116757351568";


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
               ///
            }
            else if (e.Url.Contains("error"))
            {
                await DisplayAlert("Ошибка оплаты", "Что-то пошло не так. Попробуйте ещё раз.", "OK");
                e.Cancel = true;
            }
        }
    }
}
