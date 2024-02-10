using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Producers")]
public class Producer{
    public Producer(){
    }

    public Producer(Producer producer){
        Id = producer.Id;
        Name = producer.Name;
        Email = producer.Email;
        Password = producer.Password;
        Telephone = producer.Telephone;
        WhereToFind = producer.WhereToFind;
        Location = producer.Location;
        FavdByConsumers = producer.FavdByConsumers;
        CreatedAt = producer.CreatedAt;
        UpdatedAt = producer.UpdatedAt;
        DeletedAt = producer.DeletedAt;
    }

    [Key] [Column("Id")] public Guid Id{ get; set; }

    [Required] [Column("Name")] public string Name{ get; set; }

    [Required]
    [Column("NormalizedName")]
    public string NormalizedName { get; set; }

    [Required] [Column("Email")] public string Email{ get; set; }

    [Required] [Column("Password")] public string Password{ get; set; }

    [Column(TypeName ="geography (point)")]
    [Required]
    public Point Location { get; set; }

    [Required] [Column("Telephone")] public string Telephone{ get; set; }

    [Required]
    [Column("HasProfilePicture")] public bool HasProfilePicture { get; set; }

    [Required] [Column("WhereToFind")] public string WhereToFind{ get; set; }

    [Required]
    [Column("RatingsCount")]
    public int RatingsCount { get; set; }

    [Required]
    [Column("RatingsAvg")]
    public double RatingsAvg { get; set; }

    public List<Product>? Products{ get; set; }
    public List<Order>? Orders{ get; set; }
    public List<ConsumerFavProducer> FavdByConsumers{ get; set; }
    public List<Rating>? Ratings { get; set; }

    [Column("CreatedAt")] public DateTime CreatedAt{ get; set; }

    [Column("UpdatedAt")] public DateTime UpdatedAt{ get; set; }

    [Column("DeletedAt")] public DateTime? DeletedAt{ get; set; }
}