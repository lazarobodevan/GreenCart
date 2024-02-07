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
        OriginCity = producer.OriginCity;
        Telephone = producer.Telephone;
        Picture = producer.Picture;
        CPF = producer.CPF;
        AttendedCities = producer.AttendedCities;
        WhereToFind = producer.WhereToFind;
        FavdByConsumers = producer.FavdByConsumers;
        CreatedAt = producer.CreatedAt;
        UpdatedAt = producer.UpdatedAt;
        DeletedAt = producer.DeletedAt;
    }

    [Key] [Column("Id")] public Guid Id{ get; set; }

    [Required] [Column("Name")] public string Name{ get; set; }

    [Required] [Column("Email")] public string Email{ get; set; }

    [Required] [Column("Password")] public string Password{ get; set; }

    [Required] [Column("OriginCity")] public string OriginCity{ get; set; }

    [Required] [Column("Telephone")] public string Telephone{ get; set; }

    [Column("ProfilePicture")] 
    [ForeignKey("ProducerPicture")] 
    public Guid ProfilePictureKey { get; set; }

    [Column("Picture")] public ProducerPicture? Picture{ get; set; }

    [Required] [Column("CPF")] public string CPF{ get; set; }

    [Required] [Column("AttendedCities")] public string AttendedCities{ get; set; }

    [Required] [Column("WhereToFind")] public string WhereToFind{ get; set; }

    public List<Product>? Products{ get; set; }
    public List<Order>? Orders{ get; set; }
    public List<ConsumerFavProducer> FavdByConsumers{ get; set; }

    [Column("CreatedAt")] public DateTime CreatedAt{ get; set; }

    [Column("UpdatedAt")] public DateTime UpdatedAt{ get; set; }

    [Column("DeletedAt")] public DateTime? DeletedAt{ get; set; }
}