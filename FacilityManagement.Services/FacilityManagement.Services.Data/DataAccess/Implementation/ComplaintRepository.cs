using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Implementation
{
    public class ComplaintRepository : GenericRepository<Complaint>, IComplaintRepository
    {
        private readonly DataContext _ctx;

        public ComplaintRepository(DataContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<ICollection<Complaint>> GetComplaintsByPageNumber(int pageNumber, int per_page, string categoryId)
        {
            var allFeeds = _ctx.Complaints.Where(model => model.CategoryId == categoryId).Include(model => model.User)
                .OrderByDescending(x => x.Created_at).AsQueryable();
            var pagedItems = await GetPaginated(pageNumber, per_page, allFeeds);
            return pagedItems;
        }

        public async Task<ICollection<Complaint>> GetComplaintsPaginatedByUserId(int pageNumber, int per_page, string userId)
        {
            var allComplaints = _ctx.Complaints.Where(x => x.UserId == userId).Include(u => u.User)
                .OrderByDescending(x => x.Created_at).AsQueryable();
            var pagedItems = await GetPaginated(pageNumber, per_page, allComplaints);
            return pagedItems;
        }

        public async Task<Complaint> GetComplaintById(string complaintId)
        {
            var complaint = await _ctx.Complaints.Include(model => model.User).Include(model => model.Ratings).
                Include(model => model.Comments).ThenInclude(model => model.User).
                Include(model => model.Comments).ThenInclude(model => model.Replies).
                ThenInclude(model => model.User).FirstOrDefaultAsync(complaint => complaint.Id == complaintId);

            return complaint;
        }
    }
}