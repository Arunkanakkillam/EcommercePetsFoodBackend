namespace EcommercePetsFoodBackend.Data.Dto
{
    public class LoginDto
    {
   
        public string Token { get; set; }
        public string Name {  get; set; }
        public string Role { get; set; }
        public int User_Id { get; set; }
        public string Error {  get; set; }  
    }
}
