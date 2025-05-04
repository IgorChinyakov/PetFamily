using PetFamily.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Core.Abstractions.Database
{
    public interface IVolunteersReadDbContext
    {
        IQueryable<PetDto> Pets { get; }
        IQueryable<VolunteerDto> Volunteers { get; }
    }
}
