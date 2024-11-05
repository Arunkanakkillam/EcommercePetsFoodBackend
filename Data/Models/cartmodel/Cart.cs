using EcommercePetsFoodBackend.Data.Models.Customer;

namespace EcommercePetsFoodBackend.Data.Models.cartmodel
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual Customers customer { get; set; }
        public ICollection<CartItem> Items { get; set; }
        
    }
}
