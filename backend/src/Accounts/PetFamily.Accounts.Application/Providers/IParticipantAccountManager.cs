using PetFamily.Accounts.Domain.Entities;

namespace PetFamily.Accounts.Application.Providers
{
    public interface IParticipantAccountManager
    {
        Task CreateAdminAccount(ParticipantAccount participantAccount);
    }
}