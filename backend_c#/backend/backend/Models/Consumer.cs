using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("Consumers")]
public class Consumer{
    public Consumer(){
    }


    public Consumer(Guid id, string name, string email, string password, string originCity, string telephone,
        byte[]? picture, string cPF, List<Order>? orders, List<ConsumerFavProducer>? favdProducers, DateTime createdAt,
        DateTime updatedAt, DateTime? deletedAt){
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        OriginCity = originCity;
        Telephone = telephone;
        Picture = picture;
        CPF = cPF;
        Orders = orders;
        FavdProducers = favdProducers;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public Guid Id{ get; set; }

    [Required] [Column("Name")] public string Name{ get; set; }

    [Required] [Column("Email")] public string Email{ get; set; }

    [Required] [Column("Password")] public string Password{ get; set; }

    [Required] [Column("Origin_City")] public string OriginCity{ get; set; }

    [Required] [Column("Telephone")] public string Telephone{ get; set; }

    [Column("Picture")] public byte[]? Picture{ get; set; }

    [Required] [Column("CPF")] public string CPF{ get; set; }

    public List<Order>? Orders{ get; set; }
    public List<ConsumerFavProducer>? FavdProducers{ get; set; }

    [Column("CreatedAt")] public DateTime CreatedAt{ get; set; }

    [Column("UpdatedAt")] public DateTime UpdatedAt{ get; set; }

    [Column("DeletedAt")] public DateTime? DeletedAt{ get; set; }
}