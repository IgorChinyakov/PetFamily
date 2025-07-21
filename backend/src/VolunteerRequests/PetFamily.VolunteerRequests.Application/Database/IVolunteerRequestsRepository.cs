using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.VolunteerRequests.Domain.Entities;
using PetFamily.VolunteerRequests.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Application.Database
{
    public interface IVolunteerRequestsRepository
    {
        Task<Guid> Add(VolunteerRequest request);

        Task<Result<VolunteerRequest, Error>> GetById(RequestId id);

        Task<Result<VolunteerRequest, Error>> GetByUserId(UserId userId);

        Task<bool> HasRecentRejection(UserId userId, int days);
    }
}
