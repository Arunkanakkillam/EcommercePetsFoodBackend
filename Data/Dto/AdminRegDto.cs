using System.ComponentModel.DataAnnotations;

namespace EcommercePetsFoodBackend.Data.Dto
{
    public class AdminRegDto
    {
        [Required]
        public string name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(5)]
        public string password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
