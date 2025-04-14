using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Application.Database
{
    public interface IReadDbContext
    {
        IQueryable<VolunteerDto> Volunteers { get; }
        IQueryable<PetDto> Pets { get; }
    }
}
