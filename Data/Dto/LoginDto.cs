namespace EcommercePetsFoodBackend.Data.Dto
{
    public class LoginDto
    {
        public string Email { get; set; }   
        public string Password { get; set; }
        public string Token { get; set; }  

        public string Error {  get; set; }  
    }
}
