using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; }
    }
}
