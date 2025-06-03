using PetFamily.Accounts.Domain.Entities;

namespace PetFamily.Accounts.Application.Providers
{
    public interface IVolunteerAccountManager
    {
        Task CreateVolunteerAccount(VolunteerAccount volunteerAccount);

        Task<VolunteerAccount?> FindAccountByUserId(Guid userId);
    }
}