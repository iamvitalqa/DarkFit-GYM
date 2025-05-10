using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarPage : ContentPage, INotifyPropertyChanged
    {
        private readonly string connectionString = DarkFitDatabase.ConnectionString;

        public ObservableCollection<ProductGroup> ProductGroups { get; set; }
        public Command<ProductGroup> ToggleExpandCommand { get; }

        public BarPage()
        {
            InitializeComponent();
            ProductGroups = new ObservableCollection<ProductGroup>();
            ToggleExpandCommand = new Command<ProductGroup>(OnToggleExpand);
            BindingContext = this;
            LoadCategories();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Сворачиваем все аккордеоны (группы продуктов)
            foreach (var group in ProductGroups)
            {
                group.IsExpanded = false;
            }
        }

        private async void LoadCategories()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT product_type_id, product_type_name FROM product_type";
                    var cmd = new NpgsqlCommand(query, connection);
                    var reader = await cmd.ExecuteReaderAsync();

                    var categories = new ObservableCollection<ProductGroup>();

                    while (await reader.ReadAsync())
                    {
                        categories.Add(new ProductGroup
                        {
                            ProductTypeId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Products = new ObservableCollection<Product>(),
                            IsExpanded = false
                        });
                    }

                    await reader.CloseAsync();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ProductGroups.Clear();
                        foreach (var category in categories)
                            ProductGroups.Add(category);
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private async Task LoadProductsForCategory(ProductGroup group)
        {
            if (group.Products.Any())
                return;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT productname, productcost, product_image, product_type_id FROM products WHERE product_type_id = @id";
                    var cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", group.ProductTypeId);
                    var reader = await cmd.ExecuteReaderAsync();

                    var products = new ObservableCollection<Product>();

                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            ProductName = reader.GetString(0),
                            ProductCost = reader.GetDecimal(1),
                            ProductImage = reader.GetString(2),
                            ProductTypeId = reader.GetInt32(3)
                        });
                    }

                    await reader.CloseAsync();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var p in products)
                            group.Products.Add(p);
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        private async void OnToggleExpand(ProductGroup group)
        {
            if (group == null) return;

            group.IsExpanded = !group.IsExpanded;

            if (group.IsExpanded && !group.Products.Any())
                await LoadProductsForCategory(group);
        }

        private async void BuyButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Product product)
            {
                CartService.AddToCart(product);
                await ShowToast($"Товар «{product.ProductName}» добавлен в корзину!");
            }
        }

        private async Task ShowToast(string message)
        {
            ToastLabel.Text = message;
            ToastFrame.Opacity = 0;
            ToastFrame.IsVisible = true;

            await ToastFrame.FadeTo(1, 250, Easing.SinIn);
            await Task.Delay(2000);
            await ToastFrame.FadeTo(0, 250, Easing.SinOut);

            ToastFrame.IsVisible = false;
        }

        private async void cartButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(BuyPage));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class ProductGroup : INotifyPropertyChanged
    {
        public int ProductTypeId { get; set; }
        public string Name { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Product> Products { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Product
    {
        public string ProductName { get; set; }
        public decimal ProductCost { get; set; }
        public string ProductImage { get; set; }
        public int ProductTypeId { get; set; }
    }
}
