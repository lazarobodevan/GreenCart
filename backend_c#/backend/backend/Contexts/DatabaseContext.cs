using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Contexts;

public class DatabaseContext : DbContext, IDatabaseContextOptions{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){
    }

    public DatabaseContext(){
    }

    public DbSet<Models.Producer> Producers{ get; set; }
    public DbSet<Consumer> Consumers{ get; set; }
    public DbSet<Order> Orders{ get; set; }
    public DbSet<Models.Product> Products{ get; set; }
}