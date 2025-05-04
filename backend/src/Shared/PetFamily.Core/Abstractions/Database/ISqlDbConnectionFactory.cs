using System.Data;

namespace PetFamily.Core.Abstractions.Database
{
    public interface ISqlDbConnectionFactory
    {
        IDbConnection Create();
    }
}
