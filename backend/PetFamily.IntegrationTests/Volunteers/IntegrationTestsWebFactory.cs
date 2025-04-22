using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Database;
using PetFamily.Infrastructure.DbContexts;
using Testcontainers.PostgreSql;

namespace PetFamily.IntegrationTests.Volunteers
{
    public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres")
            .WithDatabase("tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureDefaultServices);
        }

        protected virtual void ConfigureDefaultServices(IServiceCollection services)
        {
            var writeContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(WriteDbContext));

            var readContext = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IReadDbContext));

            if (writeContext is not null)
                services.Remove(writeContext);

            if (readContext is not null)
                services.Remove(readContext);

            services.AddScoped<WriteDbContext>(_ =>
                new WriteDbContext(_dbContainer.GetConnectionString()));

            services.AddScoped<IReadDbContext>(_ =>
                new ReadDbContext(_dbContainer.GetConnectionString()));
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _dbContainer.DisposeAsync();
        }
    }
}
