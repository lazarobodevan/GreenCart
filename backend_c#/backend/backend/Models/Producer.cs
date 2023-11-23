using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("Producers")]
    public class Producer{
        
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Required]
        [Column("Name")]
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
        public String CPF {  get; set; }

        [Required]
        [Column("AttendedCities")]
        public String AttendedCities {  get; set; }

        [Required]
        [Column("WhereToFind")]
        public String WhereToFind {  get; set; }

        public List<Product>? Products { get; set; }
        public List<Order>? Orders { get; set; }
        public List<ConsumerFavProducer> FavdByConsumers { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt {  get; set; }

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

        [Column("DeletedAt")]
        public DateTime? DeletedAt { get; set; }

        public Producer() { }

        public Producer(Guid id, string name, string email, string password, string origin_City, string telephone, byte[]? picture, string cPF, string attended_Cities, string where_to_Find, DateTime createdAt, DateTime updatedAt, DateTime? deletedAt) {
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
    }
}