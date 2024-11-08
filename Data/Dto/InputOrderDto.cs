namespace EcommercePetsFoodBackend.Data.Dto
{
    public class InputOrderDto
    {
        public int User_Id { get; set; }
        public string DeliveryAddres {  get; set; }
        public long Phone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
