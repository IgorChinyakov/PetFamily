using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;

namespace PetFamily.Application.Volunteers
{
    public interface IVolunteerRepository
    {
        Task<Guid> Add(Volunteer volunteer, CancellationToken token = default);

        Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber phoneNumber, CancellationToken token = default);

        Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken token = default);

        Task<Result<Pet, Error>> GetPetById(Guid volunteerId, Guid petId, CancellationToken token);

        Guid Save(Volunteer volunteer, CancellationToken token = default);

        Guid SoftDelete(Volunteer volunteer, CancellationToken token = default);

        Guid HardDelete(Volunteer volunteer, CancellationToken token = default);
    }
}