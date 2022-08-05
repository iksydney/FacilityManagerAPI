using FacilityManagement.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacilityManagement.Services.DTOs.ManualMappers
{
    public class PaginationMappers
    {
        /// <summary>
        /// Maps the main category object to the FeedDTO
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static ICollection<ReturnedFeedDTO> ForCategory(ICollection<Category> categories)
        {
            var result = new List<ReturnedFeedDTO>();
            foreach (var category in categories)
            {
                result.Add(new ReturnedFeedDTO
                {
                    Id = category.Id,
                    Description = category.Description,
                    Name = category.Name
                });
            }
            return result;
        }

        /// <summary>
        /// Maps a collection of the main Complaints model to a collection of the PaginatedComplaintsDTO
        /// </summary>
        /// <param name="complaints"></param>
        /// <returns></returns>
        public static ICollection<PaginatedComplaintsDTO> ForPaginatedComplaints(ICollection<Complaint> complaints)
        {
            var result = new List<PaginatedComplaintsDTO>();
            foreach (var complaint in complaints)
            {
                result.Add(new PaginatedComplaintsDTO
                {
                    Id = complaint.Id,
                    Image = complaint.AvatarUrl,
                    Question = complaint.Question,
                    Title = complaint.Title,
                    Type = complaint.Type,
                    Squad = complaint.User.Squad,
                    Stack = complaint.User.Stack,
                    AvatarUrl = complaint.User.AvatarUrl,
                    FirstName = complaint.User.FirstName,
                    LastName = complaint.User.LastName,
                    Time_created = complaint.Created_at
                });
            }
            return result;
        }

        /// <summary>
        /// Maps the main Complaint model to a ComplaintDTO
        /// </summary>
        /// <param name="complaint"></param>
        /// <returns></returns>
        public static ComplaintsDTO ForComplaints(Complaint complaint)
        {
            return (new ComplaintsDTO
            {
                AvatarUrl = complaint.AvatarUrl ?? null,
                PublicId = complaint.PublicId ?? null,
                Question = complaint.Question ?? null,
                Title = complaint.Title ?? null,
                Type = complaint.Type ?? null,
                Comments = complaint.Comments != null ? ForCommentComplaints(complaint.Comments): null,
                TotalRatingCount = complaint.Ratings != null ? complaint.Ratings.Count() : 0,
                Ratings = complaint.Ratings != null ? ForRatings(complaint.Ratings): null,
                User = complaint.User != null ? new ComplaintUserDTO { 
                    FirstName = complaint.User.FirstName, 
                    LastName = complaint.User.LastName,
                    AvatarUrl = complaint.User.AvatarUrl, 
                    PublicId = complaint.User.PublicId,
                    Squad = complaint.User.Squad,
                    Stack = complaint.User.Stack
                } : null,
                IsTask = complaint.IsTask
            });
        }

        private static ICollection<ComplaintRatingsDTO> ForRatings(ICollection<Ratings> ratings)
        {
            List<ComplaintRatingsDTO> allRatings = new List<ComplaintRatingsDTO>();
            foreach (var rating in ratings)
            {
                allRatings.Add(new ComplaintRatingsDTO
                {
                    ComplaintId = rating.ComplaintId,
                    Rating = rating.Rating,
                    UserId = rating.UserId,
                    TimeCreated= rating.Created_at
                });
            }

          return  allRatings.OrderByDescending(rating => rating.TimeCreated).ToList();
        }

        /// <summary>
        /// Maps the main Comment model to a ComplaintCommentDTO
        /// </summary>
        /// <param name="comments"></param>
        /// <returns></returns>
        private static ICollection<ComplaintCommentsDTO> ForCommentComplaints(ICollection<Comments> comments)
        {
            var commentsToDTO = new List<ComplaintCommentsDTO>();
            foreach (var comment in comments)
            {
                commentsToDTO.Add(new ComplaintCommentsDTO
                {
                    Comment = comment.Comment,
                    Time_created = comment.Created_at,
                    User =  comment.User!= null? new ComplaintUserDTO
                    {
                        AvatarUrl = comment.User.AvatarUrl ?? null,
                        FirstName = comment.User.FirstName ?? null,
                        LastName = comment.User.LastName ?? null,
                        Squad = comment.User.Squad ?? null,
                        Stack = comment.User.Stack ?? null
                    }: null,
                    Replies = comment.Replies != null? ForComplaintReplies(comment.Replies): null
                });
            }

            return commentsToDTO.OrderByDescending(comm=>comm.Time_created).ToList();
        }

        /// <summary>
        /// Maps the main Comment model to a ComplaintReplyDTO
        /// </summary>
        /// <param name="replies"></param>
        /// <returns></returns>
        private static ICollection<ComplaintsReplyDTO> ForComplaintReplies(ICollection<Replies> replies)
        {
            var repliesToDTO = new List<ComplaintsReplyDTO>();
            foreach (var reply in replies)
            {
                repliesToDTO.Add(new ComplaintsReplyDTO
                {
                    Reply = reply.Reply?? null,
                    Time_created = reply.Created_at,
                    User = reply.User != null? new ComplaintUserDTO
                    {
                        AvatarUrl = reply.User.AvatarUrl ?? null,
                        FirstName = reply.User.FirstName ?? null,
                        LastName = reply.User.LastName ?? null,
                        Squad = reply.User.Squad ?? null,
                        Stack = reply.User.Stack ?? null
                    }: null
                });
            }
            return repliesToDTO.OrderByDescending(reply=>reply.Time_created).ToList();
        }

        public static ICollection<UserDTO> ForUsers(IEnumerable<User> users)
        {
            var result = new List<UserDTO>();
            foreach (var user in users)
            {
                result.Add(new UserDTO
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    AvatarUrl = user.AvatarUrl,
                    Gender = user.Gender,
                    IsActive = user.IsActive,
                    IsProfileCompleted = user.IsProfileCompleted,
                    PhoneNumber = user.PhoneNumber,
                    Squad = user.Squad,
                    Stack = user.Stack,
                    Updated_at = user.Updated_at,
                    Created_at = user.Created_at,
                });
            }
            return result;
        }
    }
}
