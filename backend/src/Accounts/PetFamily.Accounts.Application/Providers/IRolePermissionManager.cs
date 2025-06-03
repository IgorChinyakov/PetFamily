namespace PetFamily.Accounts.Application.Providers
{
    public interface IRolePermissionManager
    {
        Task AddRangeIfExists(Guid roleId, IEnumerable<string> permissions);
    }
}