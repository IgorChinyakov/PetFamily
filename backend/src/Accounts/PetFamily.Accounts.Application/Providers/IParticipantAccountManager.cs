using PetFamily.Accounts.Domain.Entities;

namespace PetFamily.Accounts.Application.Providers
{
    public interface IParticipantAccountManager
    {
        Task CreateParticipantAccount(ParticipantAccount participantAccount);

        Task<ParticipantAccount?> FindAccountByUserId(Guid userId);
    }
}