using PetFamily.Core.DTOs;
using PetFamily.Volunteers.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Volunteers.Application.Database
{
    public interface IVolunteersReadDbContext
    {
        IQueryable<PetDto> Pets { get; }
        IQueryable<VolunteerDto> Volunteers { get; }
    }
}
