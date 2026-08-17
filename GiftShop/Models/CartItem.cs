namespace GiftShop.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ProductImage { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        // GST percentage for this particular product
        public decimal GstPercentage { get; set; }

        // Shipping charge for this particular product
        public decimal ShippingCharge { get; set; }

        // Product total before GST and shipping
        public decimal Total
        {
            get
            {
                return Price * Quantity;
            }
        }

        // GST amount for this product
        public decimal GSTAmount
        {
            get
            {
                return Total * GstPercentage / 100m;
            }
        }

        // Shipping amount for this product
        public decimal ShippingAmount
        {
            get
            {
                return ShippingCharge * Quantity;
            }
        }

        // Final total for this product
        public decimal GrandTotal
        {
            get
            {
                return Total + GSTAmount + ShippingAmount;
            }
        }
    }
}