using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Infrastructure.Repositories
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly WriteDbContext _context;

        public VolunteerRepository(WriteDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Add(Volunteer volunteer, CancellationToken token = default)
        {
            await _context.Volunteers.AddAsync(volunteer, token);

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

        public Guid Save(
            Volunteer volunteer, CancellationToken token = default)
        {
            _context.Volunteers.Attach(volunteer);

            return volunteer.Id;
        }

        public Guid SoftDelete(
            Volunteer volunteer, CancellationToken token = default)
        {
            volunteer.Delete();

            return volunteer.Id;
        }

        public Guid HardDelete(
            Volunteer volunteer, CancellationToken token = default)
        {
            _context.Remove(volunteer);

            return volunteer.Id;
        }
    }
}
