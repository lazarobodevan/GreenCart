using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Contexts {
    public class DatabaseContext: DbContext {

        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options) {

        }

        public DatabaseContext() { }

        public DbSet<Producer> Producers { get; set; }
        public DbSet<Consumer> Consumers { get;}     
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
