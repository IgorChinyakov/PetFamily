using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Core.Abstractions.Database;
using System.Data;

namespace PetFamily.Accounts.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AccountDbContext _dbContext;

        public UnitOfWork(AccountDbContext dbContext)
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
