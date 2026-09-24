using Microsoft.EntityFrameworkCore;

namespace psH60A01.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateFulfilled { get; set; }
        [Precision(10, 2)]
        public decimal Total { get; set; }
        [Precision(8,2)]
        public decimal Taxes { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
