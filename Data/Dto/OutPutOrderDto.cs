namespace EcommercePetsFoodBackend.Data.Dto
{
    public class OutPutOrderDto
    {
        public int Id { get; set; }
        public int ProductId {  get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public int UserId {  get; set; }
        public int orderId { get; set; }
        public DateTime CreatedDate { get; set; }   
        public string image { get; set; }
    }
}
