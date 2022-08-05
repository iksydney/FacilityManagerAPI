using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface IComplaintService
    {
        Task<Response<ComplaintsDTO>> GetComplaintByComplaintId(string complaintId);
        Task<Response<Pagination<PaginatedComplaintsDTO>>> GetComplaintsByPage(int pageNumber, string categoryId);
        Task<Response<Pagination<PaginatedComplaintsDTO>>> GetUserComplaintsByUserId(int pageNumber, string userId);
        Task<Response<ComplaintResponseDTO>> AddComplaint(string feedId, AddComplaintDTO complaint);
        Task<Response<string>> EditComplaint(string complaintId, EditComplaintDTO complaint);
        Task<Response<string>> DeleteComplaint(string complaintId);
        Task<Response<ImageAddedDTO>> UploadImage(AddImageDTO model);
    }
}
