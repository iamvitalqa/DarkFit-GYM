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

        public string BonusOptionText => $"Оплатить используя баланс: {ClientBalance:C}";

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
            var userId = Preferences.Get("UserId", 0);
            if (userId == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пользователь не найден", "OK");
                return;
            }

            if (IsBonusSelected)
            {
                if (ClientBalance >= TotalCost)
                {
                    try
                    {
                        string clientName = string.Empty;

                        using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                        {
                            await conn.OpenAsync();

                            // Получение имени клиента
                            using (var cmdName = new NpgsqlCommand("SELECT clientname FROM clients WHERE user_id = @id", conn))
                            {
                                cmdName.Parameters.AddWithValue("id", userId);
                                var result = await cmdName.ExecuteScalarAsync();
                                clientName = result?.ToString() ?? "Клиент";
                            }


                            // Списание средств
                            using (var cmd = new NpgsqlCommand("UPDATE clients SET clientbalance = clientbalance - @amount WHERE client_id = @id", conn))
                            {
                                cmd.Parameters.AddWithValue("amount", TotalCost);
                                cmd.Parameters.AddWithValue("id", userId);
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }

                        ClientBalance -= TotalCost;


                        string productList = string.Join(", ", CartItems.Select(item => item.ProductName));
                        await SendNotificationToAdminsAsync($"{clientName} оплатил(а) заказ: {productList}, на сумму {TotalCost:C}");

                        await Application.Current.MainPage.DisplayAlert("Успех", $"Оплата бонусами прошла успешно!", "OK");
                        CartItems.Clear();
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось выполнить оплату: {ex.Message}", "OK");
                    }
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

        private async Task SendNotificationToAdminsAsync(string message)
        {
            try
            {
                var userId = Preferences.Get("UserId", 0);
                if (userId == 0) return;

                using (var conn = new NpgsqlConnection(DarkFitDatabase.ConnectionString))
                {
                    await conn.OpenAsync();

                    string getAdminsQuery = "SELECT user_id FROM users WHERE role_id = 1";
                    using (var getAdminsCmd = new NpgsqlCommand(getAdminsQuery, conn))
                    using (var reader = await getAdminsCmd.ExecuteReaderAsync())
                    {
                        var adminUserIds = new System.Collections.Generic.List<int>();
                        while (await reader.ReadAsync())
                            adminUserIds.Add(reader.GetInt32(0));

                        reader.Close();

                        foreach (var adminId in adminUserIds)
                        {
                            string insertQuery = @"
                                INSERT INTO notifications (sender_user_id, user_id, message, created_at, is_read)
                                VALUES (@sender_user_id, @receiver_user_id, @message, @created_at, false)";

                            using (var insertCmd = new NpgsqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("sender_user_id", userId);
                                insertCmd.Parameters.AddWithValue("receiver_user_id", adminId);
                                insertCmd.Parameters.AddWithValue("message", message);
                                insertCmd.Parameters.AddWithValue("created_at", DateTime.UtcNow);
                                await insertCmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при отправке уведомления: " + ex.Message);
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
