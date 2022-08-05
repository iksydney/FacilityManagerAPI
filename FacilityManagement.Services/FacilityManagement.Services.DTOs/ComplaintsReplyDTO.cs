using System;

namespace FacilityManagement.Services.DTOs
{
    public class ComplaintsReplyDTO
    {
        public string Reply { get; set; }
        public string UserId { get; set; }

        public DateTime Time_created { get; set; }
        public ComplaintUserDTO User { get; set; }
    }
}
