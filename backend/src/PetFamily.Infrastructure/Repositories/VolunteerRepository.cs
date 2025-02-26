using PetFamily.Application.Volunteers;
using PetFamily.Domain.VolunteerContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure.Repositories
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private ApplicationDbContext _context;

        public VolunteerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Add(Volunteer volunteer, CancellationToken token = default)
        {
            await _context.Volunteers.AddAsync(volunteer, token);
            await _context.SaveChangesAsync(token);

            return volunteer.Id;
        }
    }
}
