using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure.Authorization.Managers
{
    public class ParticipantAccountManager : IParticipantAccountManager
    {
        private readonly AccountDbContext _context;

        public ParticipantAccountManager(AccountDbContext context)
        {
            _context = context;
        }

        public async Task CreateAdminAccount(ParticipantAccount participantAccount)
        {
            await _context.ParticipantAccounts.AddAsync(participantAccount);
            await _context.SaveChangesAsync();
        }
    }
}
