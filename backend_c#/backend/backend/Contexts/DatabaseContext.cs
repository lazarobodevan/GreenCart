using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Contexts;

public class DatabaseContext : DbContext, IDatabaseContextOptions{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){
    }

    public DatabaseContext(){
    }

    public virtual DbSet<Models.Producer> Producers{ get; set; }
    public virtual DbSet<Consumer> Consumers{ get; set; }
    public virtual DbSet<Order> Orders{ get; set; }
    public virtual DbSet<Models.Product> Products{ get; set; }
}