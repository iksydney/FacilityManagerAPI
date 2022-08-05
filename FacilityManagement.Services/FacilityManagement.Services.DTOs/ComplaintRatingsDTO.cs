using System;

namespace FacilityManagement.Services.DTOs
{
    public class ComplaintRatingsDTO
    {
        public int Rating { get; set; }

        public string UserId { get; set; }

        public string ComplaintId { get; set; }

        public DateTime TimeCreated { get; set; }
    }
}