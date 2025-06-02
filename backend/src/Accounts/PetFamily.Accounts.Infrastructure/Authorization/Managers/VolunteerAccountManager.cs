using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure.Authorization.Managers
{
    public class VolunteerAccountManager : IVolunteerAccountManager
    {
        private readonly AccountDbContext _context;

        public VolunteerAccountManager(AccountDbContext context)
        {
            _context = context;
        }

        public async Task CreateVolunteerAccount(VolunteerAccount volunteerAccount)
        {
            await _context.VolunteerAccounts.AddAsync(volunteerAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<VolunteerAccount?> FindAccountByUserId(Guid userId)
        {
            return await _context.VolunteerAccounts
                .FirstOrDefaultAsync(va => va.UserId == userId);
        }
    }
}
