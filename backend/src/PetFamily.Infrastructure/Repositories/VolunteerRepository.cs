using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
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

        public async Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber phoneNumber)
        {
            var volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.PhoneNumber.Value == phoneNumber.Value);

            if (volunteer == null)
                return Errors.General.NotFound();

            return volunteer;
        }
    }
}
