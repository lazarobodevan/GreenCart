using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Product.Enums;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Models;

[Table("Orders")]
public class Order{
    public Order(){
    }

    public Order(Order order) {
        Id = order.Id;
        Consumer = order.Consumer;
        Producer = order.Producer;
        Product = order.Product;
        Quantity = order.Quantity;
        ConsumerObs = order.ConsumerObs;
        ProducerObs = order.ProducerObs;
        Status = order.Status;
        CreatedAt = order.CreatedAt;
        UpdatedAt = order.UpdatedAt;
        AcceptedAt = order.AcceptedAt;
        RejectedAt = order.RejectedAt;
        DeletedAt = order.DeletedAt;
    }

    [Key] [Column("Id")] public Guid Id{ get; set; }

    [ForeignKey("Consumer")] public Guid ConsumerId{ get; set; }

    public Consumer Consumer{ get; set; }

    [ForeignKey("Producer")] public Guid ProducerId{ get; set; }

    public Producer Producer{ get; set; }

    [ForeignKey("Product")] public Guid ProductId{ get; set; }

    public Product Product{ get; set; }

    [Required] [Column("Quantity")] public int Quantity{ get; set; }

    [Column("ConsumerObs")] public string? ConsumerObs{ get; set; }

    [Column("ProducerObs")] public string? ProducerObs{ get; set; }

    [Column("Status")] public Status Status{ get; set; }

    [Column("CreatedAt")] public DateTime CreatedAt{ get; set; }

    [Column("UpdatedAt")] public DateTime UpdatedAt{ get; set; }

    [Column("AcceptedAt")] public DateTime? AcceptedAt{ get; set; }

    [Column("RejectedAt")] public DateTime? RejectedAt{ get; set; }

    [Column("DeletedAt")] public DateTime? DeletedAt{ get; set; }
}