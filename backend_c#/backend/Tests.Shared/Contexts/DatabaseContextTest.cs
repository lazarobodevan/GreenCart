using backend.Contexts;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Shared.Contexts {
    public class DatabaseContextTest : DbContext, IDatabaseContextOptions {

        public DatabaseContextTest(DbContextOptions<DatabaseContextTest> options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(options => { options.EnableRetryOnFailure(); }) ;
        }

        public DbSet<Producer> Producers { get; set ; }

        public DbSet<Consumer> Consumers { get; set; }

        public DbSet<Order> Orders { get ; set ; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Picture> Pictures { get; set; }
    }
}
