using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuyPage : ContentPage
    {
        private CartViewModel viewModel;

        public BuyPage()
        {
            InitializeComponent();
            viewModel = new CartViewModel();
            BindingContext = viewModel;
        }

        private void removeFromCart_Clicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.BindingContext is Product product)
            {
                viewModel.RemoveFromCart(product);
            }
        }
    }

    public class CartViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> CartItems { get; }

        private decimal _totalCost;
        public decimal TotalCost
        {
            get => _totalCost;
            private set
            {
                _totalCost = value;
                OnPropertyChanged(nameof(TotalCost));
            }
        }

        public ICommand ProceedToPaymentCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CartViewModel()
        {
            CartItems = CartService.CartItems; // Подключаемся к сервису корзины
            ProceedToPaymentCommand = new Command(ProceedToPayment);
            UpdateTotalCost();

            CartItems.CollectionChanged += (s, e) => UpdateTotalCost();
        }

        public void RemoveFromCart(Product product)
        {
            if (product != null)
            {
                CartItems.Remove(product);
                UpdateTotalCost();
            }
        }

        private void UpdateTotalCost()
        {
            TotalCost = CartItems.Sum(p => p.ProductCost);
        }

        private async void ProceedToPayment()
        {
            await Application.Current.MainPage.DisplayAlert("Оплата", "Перенаправление к оплате...", "OK");
            await Application.Current.MainPage.Navigation.PushAsync(new PayPage(TotalCost));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
