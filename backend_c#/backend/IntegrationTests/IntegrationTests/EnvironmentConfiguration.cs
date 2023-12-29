using backend.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.IntegrationTests {
    internal class EnvironmentConfiguration {
        
        private DatabaseContext dbContext;
        string connectionString = "Host=localhost;Port=5432;Database=postgrestest;Username=admin;Password=admin;";
        private DbContextOptions<DatabaseContext> options;
        public EnvironmentConfiguration() {
           
            options = new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(connectionString).Options;
            this.dbContext = new DatabaseContext(options);
        }

        public async Task ConfigureTestDatabase() {
            using(var context = new DatabaseContext(options)) {
                await this.dbContext.Database.MigrateAsync();
                await this.dbContext.Database.EnsureCreatedAsync();
            }
        }

        public async Task DisposeAsync() {
            try {
                using (var context = new DatabaseContext(options)) {
                    await this.dbContext.Database.EnsureDeletedAsync();
                }
                dbContext.Dispose();
            }catch(Exception e) {

            }

        }
    }
}
