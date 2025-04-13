using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DarkFit_app
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarPage : ContentPage, INotifyPropertyChanged
    {
        private readonly string connectionString = DarkFitDatabase.ConnectionString;

        public ObservableCollection<CategoryViewModel> Categories { get; set; }
        public ObservableCollection<Product> AllProducts { get; set; }
        private CategoryViewModel _selectedCategory;
        public CategoryViewModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                    FilterProducts();
                }
            }
        }
        public ObservableCollection<Product> FilteredProducts { get; set; }

        public BarPage()
        {
            InitializeComponent();
            Categories = new ObservableCollection<CategoryViewModel>();
            AllProducts = new ObservableCollection<Product>();
            FilteredProducts = new ObservableCollection<Product>();
            BindingContext = this;
            LoadDataFromDatabase();
        }

        private async void LoadDataFromDatabase()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    // Загрузка категорий
                    var categoriesQuery = "SELECT product_type_id, product_type_name FROM product_type";
                    var command = new NpgsqlCommand(categoriesQuery, connection);
                    var reader = await command.ExecuteReaderAsync();
                    var categoriesList = new List<CategoryViewModel>();
                    while (await reader.ReadAsync())
                    {
                        categoriesList.Add(new CategoryViewModel
                        {
                            ProductTypeId = reader.GetInt32(0),
                            ProductTypeName = reader.GetString(1)
                        });
                    }
                    await reader.CloseAsync();

                    // Загрузка всех товаров
                    var productsQuery = "SELECT productname, productcost, product_image, product_type_id FROM products";
                    var productCommand = new NpgsqlCommand(productsQuery, connection);
                    var productReader = await productCommand.ExecuteReaderAsync();
                    var productsList = new List<Product>();
                    while (await productReader.ReadAsync())
                    {
                        productsList.Add(new Product
                        {
                            ProductName = productReader.GetString(0),
                            ProductCost = productReader.GetDecimal(1),
                            ProductImage = productReader.GetString(2),
                            ProductTypeId = productReader.GetInt32(3)
                        });
                    }
                    await productReader.CloseAsync();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Categories.Clear();
                        foreach (var category in categoriesList)
                        {
                            Categories.Add(category);
                        }

                        AllProducts.Clear();
                        foreach (var product in productsList)
                        {
                            AllProducts.Add(product);
                        }
                    });
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", ex.Message, "OK");
                }
            }
        }

        private void FilterProducts()
        {
            if (SelectedCategory != null)
            {
                var filtered = AllProducts.Where(p => p.ProductTypeId == SelectedCategory.ProductTypeId).ToList();
                Device.BeginInvokeOnMainThread(() =>
                {
                    FilteredProducts.Clear();
                    foreach (var product in filtered)
                    {
                        FilteredProducts.Add(product);
                    }
                });
            }
        }

       

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void buyButton_Clicked(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync(nameof(BuyPage));
            
            await DisplayAlert("Успех", "Товар добавлен в корзину!", "OK"); // Показываем всплывающее сообщение

        }

        private async void cartButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(BuyPage));
        }
    }

    public class CategoryViewModel
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
    }

    public class Product
    {
        public string ProductName { get; set; }
        public decimal ProductCost { get; set; }
        public string ProductImage { get; set; }
        public int ProductTypeId { get; set; }

        public ICommand BuyCommand { get; set; }

        public Product()
        {
            BuyCommand = new Command(AddToCart);
        }

        private void AddToCart()
        {
            CartService.AddToCart(this);
        }
    }
}
