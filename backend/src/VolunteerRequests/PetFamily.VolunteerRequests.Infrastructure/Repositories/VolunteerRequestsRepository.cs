using PetFamily.VolunteerRequests.Domain.Entities;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure.Repositories
{
    public class VolunteerRequestsRepository
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
    }
}
