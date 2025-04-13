using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayPage : ContentPage
    {
        public PayPage(decimal totalCost)
        {
            InitializeComponent();

            // Формируем ссылку с учетом суммы
            string paymentUrl = $"https://yoomoney.ru/to/4100116757351568?amount={totalCost:F2}";
            PaymentWebView.Source = paymentUrl;

            // Обрабатываем редиректы, если платежная система использует их
            PaymentWebView.Navigating += PaymentWebView_Navigating;
        }

        private async void PaymentWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.Contains("success")) // Например, если банк возвращает success-URL
            {
                await DisplayAlert("Оплата успешна", "Ваш платеж прошел успешно!", "OK");
                await Navigation.PopAsync(); // Закрыть страницу оплаты
            }
            else if (e.Url.Contains("error"))
            {
                await DisplayAlert("Ошибка оплаты", "Что-то пошло не так. Попробуйте еще раз.", "OK");
                e.Cancel = true; // Остановить переход, если есть ошибка
            }
        }
    }
}
