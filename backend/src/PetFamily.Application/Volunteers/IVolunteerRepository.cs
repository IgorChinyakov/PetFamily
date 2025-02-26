using PetFamily.Domain.VolunteerContext.Entities;

namespace PetFamily.Application.Volunteers
{
    public interface IVolunteerRepository
    {
        Task<Guid> Add(Volunteer volunteer, CancellationToken token = default);
    }
}