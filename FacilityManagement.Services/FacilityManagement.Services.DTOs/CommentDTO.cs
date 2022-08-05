using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class CommentDto
    {
        [Required]
        public string Comment { get; set; }
    }
}