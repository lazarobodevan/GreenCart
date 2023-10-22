using backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Producer {
    public class CreateProducerDTO {
        public String Name { get; set; }

        [Required]
        [Column("Email")]
        public String Email { get; set; }

        [Required]
        [Column("Password")]
        public String Password { get; set; }

        [Required]
        [Column("OriginCity")]
        public String OriginCity { get; set; }

        [Required]
        [Column("Telephone")]
        public String Telephone { get; set; }

        [Column("Picture")]
        public byte[]? Picture { get; set; }

        [Required]
        [Column("CPF")]
        public String CPF { get; set; }

        [Required]
        [Column("AttendedCities")]
        public String Attended_Cities { get; set; }

        [Required]
        [Column("WhereToFind")]
        public String Where_to_Find { get; set; }

        public List<Models.Product>? Products { get; set; }
        public List<Order>? Orders { get; set; }
        public List<ConsumerFavProducer> FavdByConsumers { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

        [Column("DeletedAt")]
        public DateTime? DeletedAt { get; set; }
    }
}
