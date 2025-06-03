using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure.Authorization.Managers
{
    public class AdminAccountManager : IAdminAccountManager
    {
        private readonly AccountDbContext _context;

        public AdminAccountManager(AccountDbContext context)
        {
            _context = context;
        }

        public async Task CreateAdminAccount(AdminAccount adminAccount)
        {
            await _context.AdminAccounts.AddAsync(adminAccount);
            await _context.SaveChangesAsync();
        }
    }
}
