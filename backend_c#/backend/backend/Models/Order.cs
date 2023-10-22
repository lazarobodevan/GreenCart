using backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models {

    [Table("Orders")]
    public class Order {

        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [ForeignKey("Consumer")]
        public Guid ConsumerId { get; set; }

        public Consumer Consumer { get; set; }

        [ForeignKey("Producer")]
        public Guid ProducerId { get; set; }

        public Producer Producer { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        [Column("Quantity")]
        public int Quantity { get; set; }

        [Column("ConsumerObs")]
        public string? ConsumerObs {  get; set; }

        [Column("ProducerObs")]
        public string? ProducerObs { get; set; }

        [Column("Status")]
        public Status Status { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get;set; }

        [Column("AcceptedAt")]
        public DateTime? AcceptedAt { get; set; }

        [Column("RejectedAt")]
        public DateTime? RejectedAt { get; set; }

        [Column("DeletedAt")]
        public DateTime? DeletedAt { get; set; }

        public Order() { }

        public Order(Guid id, Consumer consumer, Producer producer, Product product, int quantity, string? consumerObs, string? producerObs, Status status, DateTime createdAt, DateTime updatedAt, DateTime? acceptedAt, DateTime? rejectedAt, DateTime? deletedAt) {
            Id = id;
            Consumer = consumer;
            Producer = producer;
            Product = product;
            Quantity = quantity;
            ConsumerObs = consumerObs;
            ProducerObs = producerObs;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            AcceptedAt = acceptedAt;
            RejectedAt = rejectedAt;
            DeletedAt = deletedAt;
        }
    }
}
