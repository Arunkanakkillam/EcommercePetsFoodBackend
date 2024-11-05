namespace EcommercePetsFoodBackend.Data.Dto
{
    public class WishlistDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal price { get; set; }
        public string image { get; set; }
        public string categoryName { get; set; }

    }
}
