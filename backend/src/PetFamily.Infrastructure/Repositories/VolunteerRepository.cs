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
        private readonly ApplicationDbContext _context;

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

        public async Task<Result<Volunteer, Error>> GetByPhoneNumber(
            PhoneNumber phoneNumber, CancellationToken token = default)
        {
            var volunteer = await _context.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.PhoneNumber == phoneNumber, token);

            if (volunteer == null)
                return Errors.General.NotFound();

            return volunteer;
        }

        public async Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken token)
        {
            var volunteer = await _context.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == id, token);

            if(volunteer == null)
                return Errors.General.NotFound();

            return volunteer;
        }

        public async Task<Result<Pet, Error>> GetPetById(
            Guid volunteerId, Guid petId, CancellationToken token)
        {
            var volunteerResult = await GetById(volunteerId, token);
            if (volunteerResult.IsFailure)
                return Errors.General.NotFound(volunteerId);

            var petResult = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id == petId);
            if (petResult == null)
                return Errors.General.NotFound(petId);

            return petResult;
        }

        public async Task<Guid> Save(
            Volunteer volunteer, CancellationToken token = default)
        {
            await _context.SaveChangesAsync(token);

            return volunteer.Id;
        }

        public async Task<Guid> SoftDelete(
            Volunteer volunteer, CancellationToken token = default)
        {
            volunteer.Delete();
            await _context.SaveChangesAsync(token);

            return volunteer.Id;
        }

        public async Task<Guid> HardDelete(
            Volunteer volunteer, CancellationToken token = default)
        {
            _context.Remove(volunteer);
            await _context.SaveChangesAsync(token);

            return volunteer.Id;
        }
    }
}
