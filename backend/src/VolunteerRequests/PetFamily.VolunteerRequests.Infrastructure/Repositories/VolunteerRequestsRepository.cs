using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Application.Database;
using PetFamily.VolunteerRequests.Domain.Entities;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Repositories
{
    public class VolunteerRequestsRepository : IVolunteerRequestsRepository
    {
        private readonly VolunteerRequestsWriteDbContext _dbContext;

        public VolunteerRequestsRepository(VolunteerRequestsWriteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Add(VolunteerRequest request)
        {
            await _dbContext.VolunteerRequests.AddAsync(request);
            return request.Id.Value;
        }

        public async Task<Result<VolunteerRequest, Error>> GetByUserId(UserId userId)
        {
            var requestResult = await _dbContext.VolunteerRequests
                .FirstOrDefaultAsync(v => v.UserId == userId);

            if (requestResult == null)
                return Errors.General.NotFound();

            return requestResult;
        }

        public Task<bool> HasRecentRejection(UserId userId, int days)
        {
            return _dbContext.VolunteerRequests
                .Where(r => r.UserId == userId && r.RejectedRequest != null)
                .AnyAsync(r => (DateTime.UtcNow - r.RejectedRequest!.RejectionDate.Value).TotalDays < days);
        }

        public async Task<Result<VolunteerRequest, Error>> GetById(RequestId id)
        {
            var requestResult = await _dbContext.VolunteerRequests
                .FirstOrDefaultAsync(v => v.Id == id);

            if (requestResult == null)
                return Errors.General.NotFound();

            return requestResult;
        }
    }
}
