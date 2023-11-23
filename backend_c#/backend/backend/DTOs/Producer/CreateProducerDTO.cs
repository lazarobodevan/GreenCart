using backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Producer {
    public class CreateProducerDTO {

        [Required(ErrorMessage = "Nome é obrigatório")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ser válido")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public String Password { get; set; }

        [Required(ErrorMessage = "Cidade de origem é obrigatório")]
        public String OriginCity { get; set; }

        [Required(ErrorMessage = "Celular é obrigatório")]
        public String Telephone { get; set; }

        [Column("Picture")]
        public byte[]? Picture { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        public String CPF { get; set; }

        [Required(ErrorMessage = "Cidades atendidas é obrigatório")]
        public String AttendedCities { get; set; }

        [Required(ErrorMessage = "Onde te encontrar é obrigatório")]
        public String WhereToFind { get; set; }

        public List<Models.Product>? Products { get; set; }
        public List<Order>? Orders { get; set; }
        public List<ConsumerFavProducer>? FavdByConsumers { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
