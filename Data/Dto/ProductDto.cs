namespace EcommercePetsFoodBackend.Data.Dto
{
    public class ProductDto
    {
        public string ProductName { get; set; } 
        public decimal Price { get; set; }
        public bool IsAvailable {  get; set; }  
        public string ProductDescription { get; set; }
        public int  ProductCategoryId { get; set; }
        public int ProductId {  get; set; } 
        public int Quandity {  get; set; }
        public string Image { get; set; }

    }
}
