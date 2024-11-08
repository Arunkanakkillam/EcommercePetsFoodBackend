using EcommercePetsFoodBackend.Data.Models.Customer;
using EcommercePetsFoodBackend.Services.CustomerServices;
using System.ComponentModel.DataAnnotations;

namespace EcommercePetsFoodBackend.Data.Models.Orders
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public virtual Customers customer { get; set; }
        public ICollection<OrderItem> orderItems { get; set; }
    }
}
