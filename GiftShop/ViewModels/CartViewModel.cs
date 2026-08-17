using GiftShop.Models;

namespace GiftShop.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new();

        // Product subtotal
        public decimal SubTotal
        {
            get
            {
                return Items.Sum(x => x.Total);
            }
        }

        // Total GST from all products
        public decimal GST
        {
            get
            {
                return Items.Sum(x => x.GSTAmount);
            }
        }

        // Total shipping from all products
        public decimal Shipping
        {
            get
            {
                return Items.Sum(x => x.ShippingAmount);
            }
        }

        // Final amount
        public decimal GrandTotal
        {
            get
            {
                return SubTotal + GST + Shipping;
            }
        }
    }
}