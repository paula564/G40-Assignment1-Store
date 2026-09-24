using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace psH60A01.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [Precision(8, 2)]
        public decimal Price { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;

    }
}
