using System.ComponentModel.DataAnnotations;

namespace psH60A01.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(30)]
        public string Email { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [MaxLength(2)]
        public char Province {  get; set; }
        [MaxLength(16)]
        public string CreditCard { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
