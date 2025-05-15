using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Npgsql;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DarkFit_app
{
    public partial class BuyPage : ContentPage
    {
        private CartViewModel viewModel;

        public BuyPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CartViewModel();

            // Следим за изменениями в корзине
            viewModel.CartItems.CollectionChanged += (s, e) => UpdateCartVisibility();
            UpdateCartVisibility();
        }

        private void UpdateCartVisibility()
        {
            bool hasItems = viewModel.CartItems.Any();
            EmptyCartLabel.IsVisible = !hasItems;
            CartCollectionView.IsVisible = hasItems;
        }

        private void removeFromCart_Clicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.CommandParameter is Product product)
            {
                viewModel.RemoveFromCart(product);
            }
        }
    }

    public class CartViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Product> CartItems { get; private set; }

        private decimal _totalCost;
        public decimal TotalCost
        {
            get => _totalCost;
            set
            {
                _totalCost = value;
                OnPropertyChanged(nameof(TotalCost));
            }
        }

        private decimal _clientBalance;
        public decimal ClientBalance
        {
            get => _clientBalance;
            set
            {
                _clientBalance = value;
                OnPropertyChanged(nameof(ClientBalance));
                OnPropertyChanged(nameof(BonusOptionText));
            }
        }

        public string BonusOptionText => $"Оплатить бонусами (баланс: {ClientBalance:C})";

        private bool _isBonusSelected = true;
        public bool IsBonusSelected
        {
            get => _isBonusSelected;
            set
            {
                _isBonusSelected = value;
                OnPropertyChanged(nameof(IsBonusSelected));
                OnPropertyChanged(nameof(IsCardSelected));
            }
        }

        public bool IsCardSelected
        {
            get => !_isBonusSelected;
            set
            {
                IsBonusSelected = !value;
                OnPropertyChanged(nameof(IsCardSelected));
                OnPropertyChanged(nameof(IsBonusSelected));
            }
        }

        public ICommand ProceedToPaymentCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CartViewModel()
        {
            CartItems = CartService.CartItems;
            ProceedToPaymentCommand = new Command(OnProceedToPayment);

            UpdateTotalCost();
            LoadClientBalanceAsync();

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

        private async void OnProceedToPayment()
        {
            if (IsBonusSelected)
            {
                if (ClientBalance >= TotalCost)
                {
                    await Application.Current.MainPage.DisplayAlert("Успех", $"Оплата бонусами прошла успешно: {TotalCost:C}", "OK");
                    CartItems.Clear();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Недостаточно бонусов для оплаты", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.Navigation.PushAsync(new PayPage(TotalCost));
            }
        }

        private async void LoadClientBalanceAsync()
        {
            try
            {
                var userId = Preferences.Get("UserId", 0);
                if (userId == 0)
                {
                    Console.WriteLine("UserId не найден в Preferences.");
                    return;
                }

                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new NpgsqlCommand("SELECT clientbalance FROM clients WHERE client_id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("id", userId);
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && decimal.TryParse(result.ToString(), out var balance))
                        {
                            ClientBalance = balance;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении баланса: " + ex.Message);
            }
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
