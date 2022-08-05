using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class FeedDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
