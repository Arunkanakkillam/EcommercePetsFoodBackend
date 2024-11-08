using EcommercePetsFoodBackend.Data.Models.cartmodel;
using EcommercePetsFoodBackend.Data.Models.Orders;
using EcommercePetsFoodBackend.Data.Models.Wishlists;

namespace EcommercePetsFoodBackend.Data.Models.Customer
{
    public class Customers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }
        public virtual Cart cart { get; set; }
        public ICollection<Order> orders { get; set; }
        public ICollection<Wishlist> Wishlist { get; set; }
        
    }
}
