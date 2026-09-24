namespace psH60A01.Models
{
    public class ShoppingCart
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public DateOnly DateCreated { get; set; }
    }
}
