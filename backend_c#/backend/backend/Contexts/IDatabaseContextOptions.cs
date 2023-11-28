using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Contexts {
    public interface IDatabaseContextOptions {
        DbSet<Producer> Producers { get; set; }
        DbSet<Consumer> Consumers { get; }
        DbSet<Order> Orders { get; set; }
        DbSet<Product> Products { get; set; }
    }
}
