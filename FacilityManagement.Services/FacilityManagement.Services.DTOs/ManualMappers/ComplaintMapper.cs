using FacilityManagement.Services.Models;

namespace FacilityManagement.Services.DTOs.ManualMappers
{
    public class ComplaintMapper
    {
        public static ComplaintResponseDTO ToComplaintResponseDTO(Complaint complaint)
        {
            return new ComplaintResponseDTO
            {
                Id = complaint.Id,
                Type = complaint.Type,
                AvatarUrl = complaint.AvatarUrl,
                PublicId = complaint.PublicId,
                IsTask = complaint.IsTask,
                CategoryId = complaint.CategoryId,
                UserId = complaint.UserId,
                Comments = complaint.Comments,
                Question = complaint.Question,
                Ratings = complaint.Ratings,
                Title = complaint.Title
            };
        }

        public static Complaint FromAddComplaintDTO(string feedId, AddComplaintDTO complaint)
        {
            return new Complaint
            {
                Type = complaint.Type,
                AvatarUrl = complaint.AvatarUrl,
                PublicId = complaint.PublicId,
                IsTask = complaint.IsTask,
                Question = complaint.Question,
                UserId = complaint.UserId,
                CategoryId = feedId,
                Title = complaint.Title
            };
        }

        public static Complaint FromEditComplaintDTO(Complaint complaintToEdit, EditComplaintDTO complaint)
        {
            return new Complaint
            {
                Type = complaint.Type,
                AvatarUrl = complaint.AvatarUrl,
                IsTask = complaint.IsTask,
                Question = complaint.Question
            };
        }
    }
}
