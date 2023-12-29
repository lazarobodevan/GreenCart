using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Contexts;

public interface IDatabaseContextOptions{
    DbSet<Models.Producer> Producers{ get; set; }
    DbSet<Consumer> Consumers{ get; }
    DbSet<Order> Orders{ get; set; }
    DbSet<Models.Product> Products{ get; set; }
    DbSet<Models.Picture> Pictures { get; set; }
}