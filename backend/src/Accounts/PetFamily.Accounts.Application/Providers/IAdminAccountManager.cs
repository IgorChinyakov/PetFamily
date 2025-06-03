using PetFamily.Accounts.Domain.Entities;

namespace PetFamily.Accounts.Application.Providers
{
    public interface IAdminAccountManager
    {
        Task CreateAdminAccount(AdminAccount adminAccount);
    }
}