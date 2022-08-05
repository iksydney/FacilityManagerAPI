using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class EditCommentDTO
    {
        [Required]
        public string Comment { get; set; }
    }
}
