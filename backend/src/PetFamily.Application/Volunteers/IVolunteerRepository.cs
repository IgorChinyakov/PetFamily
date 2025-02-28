using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;

namespace PetFamily.Application.Volunteers
{
    public interface IVolunteerRepository
    {
        Task<Guid> Add(Volunteer volunteer, CancellationToken token = default);
        Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber phoneNumber);
    }
}