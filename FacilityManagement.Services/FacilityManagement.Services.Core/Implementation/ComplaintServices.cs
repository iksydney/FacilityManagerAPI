using CloudinaryDotNet.Actions;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.DTOs.ManualMappers;
using FacilityManagement.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Implementation
{
    public class ComplaintServices : IComplaintService
    {
        private readonly IComplaintRepository _complaintsRepo;
        private readonly int PerPage = 10;
        private readonly IFeedRepository _feedRepo;
        private readonly IUserRepository _userRepo;
        private readonly IImageService _imageService;

        public ComplaintServices(IServiceProvider serviceProvider)
        {
            _complaintsRepo = serviceProvider.GetRequiredService<IComplaintRepository>();
            _feedRepo = serviceProvider.GetRequiredService<IFeedRepository>();
            _userRepo = serviceProvider.GetRequiredService<IUserRepository>();
            _imageService = serviceProvider.GetRequiredService<IImageService>();
        }

        public async Task<Response<Pagination<PaginatedComplaintsDTO>>> GetComplaintsByPage(int pageNumber, string categoryId)
        {
            Response<Pagination<PaginatedComplaintsDTO>> response = new Response<Pagination<PaginatedComplaintsDTO>>();
            var complaints = await _complaintsRepo.GetComplaintsByPageNumber(pageNumber, PerPage, categoryId);
            if (complaints != null)
            {
                response.Success = true;
                response.Message = "Complaints paginated";
                response.Data = new Pagination<PaginatedComplaintsDTO>
                {
                    TotalNumberOfItems = _complaintsRepo.TotalNumberOfItems,
                    TotalNumberOfPages = _complaintsRepo.TotalNumberOfPages,
                    CurrentPage = pageNumber,
                    ItemsPerPage = PerPage,
                    Items = PaginationMappers.ForPaginatedComplaints(complaints)
                };
                return response;
            }
            response.Message = "No complaint found by page number";
            return response;
        }
         
        public async Task<Response<Pagination<PaginatedComplaintsDTO>>> GetUserComplaintsByUserId(int pageNumber, string userId)
        {
            Response<Pagination<PaginatedComplaintsDTO>> response = new Response<Pagination<PaginatedComplaintsDTO>>();
            var complaints = await _complaintsRepo.GetComplaintsPaginatedByUserId(pageNumber, PerPage, userId);
            if (complaints != null)
            {
                response.Success = true;
                response.Message = "Complaints paginated";
                response.Data = new Pagination<PaginatedComplaintsDTO>
                {
                    TotalNumberOfItems = _complaintsRepo.TotalNumberOfItems,
                    TotalNumberOfPages = _complaintsRepo.TotalNumberOfPages,
                    CurrentPage = pageNumber,
                    ItemsPerPage = PerPage,
                    Items = PaginationMappers.ForPaginatedComplaints(complaints)
                };
            }
            else
                response.Message = $"No complaint found for user with id {userId} by page number";

            return response;
        }

        public async Task<Response<ComplaintsDTO>> GetComplaintByComplaintId(string complaintId) 
        {
            Response<ComplaintsDTO> response = new Response<ComplaintsDTO>();
            var complaint = await _complaintsRepo.GetComplaintById(complaintId);
            if (complaint == null)
            {
                response.Message = "Complaint Id not found";
                return response;
            }

            response.Success = true;
            response.Message = "Complaint";
            response.Data = PaginationMappers.ForComplaints(complaint);
            return response;
        }

        /// <summary>
        /// Adds a complaint to an existing feed
        /// </summary>
        /// <param name="feedId"></param>
        /// <param name="complaint"></param>
        /// <returns></returns>
        public async Task<Response<ComplaintResponseDTO>> AddComplaint(string feedId, AddComplaintDTO complaint)
        {
            var response = new Response<ComplaintResponseDTO>();

            var user = await _userRepo.GetById(complaint.UserId);

            if (user == null)
            {
                response.Message = "User not found";
                return response;
            }

            var feed = await _feedRepo.GetById(feedId);
            //ends the process if the feed cannot be found
            if (feed == null)
            {
                response.Message = "Feed not found";
                response.Success = false;
                return response;
            }

            //map values of the DTO to the actual object
            var newComplaint = ComplaintMapper.FromAddComplaintDTO(feedId, complaint);

            var result = await _complaintsRepo.Add(newComplaint);

            response.Success = result;

            response.Message = !response.Success ? "Error occured while updating your entry" : "Updated successfully";
            response.Data = ComplaintMapper.ToComplaintResponseDTO(newComplaint);

            //returns the complaint added and the related field
            return response;
        }

        /// <summary>
        /// Updates a complaint in the database
        /// </summary>
        /// <param name="feedId"></param>
        /// <param name="complaintId"></param>
        /// <param name="complaint"></param>
        /// <returns></returns>
        public async Task<Response<string>> EditComplaint(string complaintId, EditComplaintDTO complaint)
        {
            var response = new Response<string>();

            var user = await _userRepo.GetById(complaint.UserId);

            if (user == null)
            {
                response.Message = "User not found";
                return response;
            }

            var complaintToEdit = await _complaintsRepo.GetById(complaintId);
            //end the process if the complaint is not found
            if (complaintToEdit == null)
            {
                response.Message = "Error retrieving complaint";
                return response;
            }

            //updates dynamic values of the complaint
            ComplaintMapper.FromEditComplaintDTO(complaintToEdit, complaint);

            //updates the status of the entity class
            var result = await _complaintsRepo.Modify(complaintToEdit);

            if (result)
            {
                response.Message = "Updated successfully";
                response.Success = true;
            }
            else
                response.Message = "Failed to update complaint";

            return response;
        }

        public async Task<Response<string>> DeleteComplaint(string complaintId)
        {
            Response<string> response = new Response<string>();
            var complaint = await _complaintsRepo.GetComplaintById(complaintId);
            if (complaint == null)
            {
                response.Message = "Invalid complaint Id";
                return response;
            }
            if (await _complaintsRepo.DeleteById(complaintId))
            {
                response.Success = true;
                response.Message = "Complaint deleted successfully";
                return response;
            }
            response.Message = "Unable to delete complaint, Plese try again later";
            return response;
        }

        public async Task<Response<ImageAddedDTO>> UploadImage(AddImageDTO model)
        {
            Response<ImageAddedDTO> response = new Response<ImageAddedDTO>();
            UploadResult result;

            try
            {
                result = await _imageService.UploadImage(model.Image);
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                return response;
            }

            string publicId = result.PublicId;
            string url = result.Url.ToString();

            if (publicId == null || url == null)
            {
                response.Message = "An error occured while trying to upload your image";
                return response;
            }

            var responseDTO = new ImageAddedDTO
            {
                PublicId = publicId,
                Url = url
            };

            response.Message = "Image Uploaded successfully!";
            response.Success = true;
            response.Data = responseDTO;

            return response;
        }
    }
}
