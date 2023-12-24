using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Producers")]
public class Producer{
    public Producer(){
    }

    public Producer(Guid id, string name, string email, string password, string origin_City, string telephone,
        byte[]? picture, string cPF, string attended_Cities, string where_to_Find, DateTime createdAt,
        DateTime updatedAt, DateTime? deletedAt){
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        OriginCity = origin_City;
        Telephone = telephone;
        Picture = picture;
        CPF = cPF;
        AttendedCities = attended_Cities;
        WhereToFind = where_to_Find;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
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

    [Column("Picture")] public byte[]? Picture{ get; set; }

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