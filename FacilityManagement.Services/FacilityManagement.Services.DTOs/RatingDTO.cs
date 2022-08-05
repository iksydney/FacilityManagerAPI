using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class RatingDTO
    {
        [Required]
        public int Rating { get; set; }
    }
}
