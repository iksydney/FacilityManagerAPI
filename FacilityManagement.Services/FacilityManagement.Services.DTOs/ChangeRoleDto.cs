using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class ChangeRoleDto
    {
        [Required]
        public string Role { get; set; }
        [Required]
        public string CurrentRole { get; set; }
    }
}
