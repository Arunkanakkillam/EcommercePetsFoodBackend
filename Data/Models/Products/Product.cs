using EcommercePetsFoodBackend.Data.Models.cartmodel;
using EcommercePetsFoodBackend.Data.Models.Wishlists;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePetsFoodBackend.Data.Models.Products
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        public string ProductDescription { get; set; }
       
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Quandity { get; set; }
        
        
        [Required]
        [ForeignKey("Category")]
        public int ProductCategoryId { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<CartItem> Items {  get; set; } 
        public ICollection<Wishlist> wishlists { get; set; }


    }
}
