using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Domain.Entities;
using PetFamily.SharedKernel;
using System.Threading.Tasks;

namespace PetFamily.Accounts.Infrastructure.Authorization.Managers
{
    public class RefreshSessionManager : IRefreshSessionManager
    {
        private readonly AccountDbContext _context;

        public RefreshSessionManager(AccountDbContext context)
        {
            _context = context;
        }

        public async Task<Result<RefreshSession, Error>> GetByRefreshToken(
            Guid refreshToken,
            CancellationToken cancellationToken = default)
        {
            var refreshSession = await _context.RefreshSessions
                .Include(rs => rs.User)
                .FirstOrDefaultAsync(rs => rs.RefreshToken == refreshToken);

            if (refreshSession is null)
                return Errors.General.NotFound(refreshToken);

            return refreshSession;
        }

        public async Task Delete(
           RefreshSession refreshSession, CancellationToken cancellationToken = default)
        {
            _context.RefreshSessions.Remove(refreshSession);
            await _context.SaveChangesAsync();
        }
    }
}
