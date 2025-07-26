using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Abstractions.Database;
using PetFamily.VolunteerRequests.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetFamily.VolunteerRequests.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VolunteerRequestsWriteDbContext _dbContext;

        public UnitOfWork(VolunteerRequestsWriteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            return transaction.GetDbTransaction();
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
