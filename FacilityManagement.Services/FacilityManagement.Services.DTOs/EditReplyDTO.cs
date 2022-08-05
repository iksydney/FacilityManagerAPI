using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class EditReplyDTO
    {
        [Required]
        public string Reply { get; set; }
    }
}
