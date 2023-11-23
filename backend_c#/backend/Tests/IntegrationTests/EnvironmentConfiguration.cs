using backend.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.IntegrationTests {
    internal class EnvironmentConfiguration {
        
        private DatabaseContext dbContext;
        string connectionString = "Host=localhost;Port=5432;Database=postgres_test;Username=admin;Password=admin;";
        public EnvironmentConfiguration() {
           
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(connectionString).Options;
            this.dbContext = new DatabaseContext(options);
        }

        public async Task ConfigureTestDatabase() {
            
            await this.dbContext.Database.MigrateAsync();

            var services = new ServiceCollection();
            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));

            services.BuildServiceProvider();

        }

        public async Task DisposeAsync() {
            await this.dbContext.Database.EnsureDeletedAsync();
            dbContext.Dispose();
        }
    }
}
