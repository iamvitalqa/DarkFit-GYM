using System.Collections.ObjectModel;

namespace DarkFit_app
{
    public static class CartService
    {
        public static ObservableCollection<Product> CartItems { get; set; } = new ObservableCollection<Product>();

        public static void AddToCart(Product product)
        {
            CartItems.Add(product);
        }
    }
}
