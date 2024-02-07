using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Contexts;

public class DatabaseContext : DbContext, IDatabaseContextOptions{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){
    }

    public DatabaseContext(){
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        //Position must be unique between pictures of a product
        modelBuilder.Entity<Models.ProductPicture>()
            .HasIndex(p => new { p.ProductId, p.Position })
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }


    public virtual DbSet<Models.Producer> Producers{ get; set; }
    public virtual DbSet<Models.Consumer> Consumers{ get; set; }
    public virtual DbSet<Models.Order> Orders{ get; set; }
    public virtual DbSet<Models.Product> Products{ get; set; }
    public virtual DbSet<Models.ProductPicture> Pictures { get; set; }
}