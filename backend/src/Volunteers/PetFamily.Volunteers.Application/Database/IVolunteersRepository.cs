using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.SharedVO;

namespace PetFamily.Volunteers.Application.Database
{
    public interface IVolunteersRepository
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