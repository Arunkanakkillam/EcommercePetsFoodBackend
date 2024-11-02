using System.ComponentModel.DataAnnotations;

namespace EcommercePetsFoodBackend.Data.Dto
{
    public class AdminRegDto
    {
        [Required]
        public string name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
      
        [Required]
        public string Role { get; set; }
    }
}
