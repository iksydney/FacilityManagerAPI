using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class AddReplyDTO
    {
        [Required]
        public string Reply { get; set; }
    }
}
