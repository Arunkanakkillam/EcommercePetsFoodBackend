using System.ComponentModel.DataAnnotations;

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
        public int ProductCategoryId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Quandity { get; set; }
        public virtual Category category { get; set; }

    }
}
