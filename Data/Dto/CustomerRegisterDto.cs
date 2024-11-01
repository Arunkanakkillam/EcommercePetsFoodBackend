using System.ComponentModel.DataAnnotations;

namespace EcommercePetsFoodBackend.Data.Dto
{
    public class CustomerRegisterDto
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public long Phone { get; set; }
        
        [MinLength(5)]
        public string Password { get; set; }

    }
}
