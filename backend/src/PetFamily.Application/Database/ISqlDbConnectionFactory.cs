using System.Data;

namespace PetFamily.Application.Database
{
    public interface ISqlDbConnectionFactory
    {
        IDbConnection Create();
    }
}
