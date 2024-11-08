using EcommercePetsFoodBackend.Data.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace EcommercePetsFoodBackend.Data.Models.Orders
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int OrderId {  get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; } 
        public string DeliveryAddress {  get; set; }
        public int Quantity { get; set; }   
        public long Phone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual Order orders { get; set; }
        public virtual Product Product { get; set; }
    }
}
