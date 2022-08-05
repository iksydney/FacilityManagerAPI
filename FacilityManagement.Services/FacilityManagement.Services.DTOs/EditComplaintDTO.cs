using FacilityManagement.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class EditComplaintDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Question { get; set; }

        [Required]
        [StringLength(500)]
        public string Type { get; set; }

        public string AvatarUrl { get; set; }

        public string PublicId { get; set; }


        public bool IsTask { get; set; }
    }
}
