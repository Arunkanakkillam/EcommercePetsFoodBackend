namespace EcommercePetsFoodBackend.Data.Models.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public DateTime OrderDate { get; set; }

    }
}
