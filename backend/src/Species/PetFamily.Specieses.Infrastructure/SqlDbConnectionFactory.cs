using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Core.Abstractions.Database;
using System.Data;

namespace PetFamily.Specieses.Infrastructure
{
    public class SqlDbConnectionFactory : ISqlDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqlDbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection Create()
            => new NpgsqlConnection(_configuration.GetConnectionString("Database"));
    }
}
