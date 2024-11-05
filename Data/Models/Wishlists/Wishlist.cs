using EcommercePetsFoodBackend.Data.Models.Customer;
using EcommercePetsFoodBackend.Data.Models.Products;

namespace EcommercePetsFoodBackend.Data.Models.Wishlists
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int ProductId { get; set; }
        public virtual Customers customer { get; set; }
        public virtual Product product { get; set; }
    }
}
