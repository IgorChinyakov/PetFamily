using PetFamily.VolunteerRequests.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Database
{
    public interface IVolunteerRequestsReadDbContext
    {
        IQueryable<VolunteerRequestDto> RequestDtos { get; }

        IQueryable<RejectedRequestDto> RejectedRequestsDtos { get; }
    }
}
