using Microsoft.EntityFrameworkCore;

namespace psH60A01.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [Precision(8, 2)]
        public decimal Price { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
