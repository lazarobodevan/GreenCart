using backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Producer {
    public class UpdateProducerDTO {

        public Guid Id { get; set; }

        public String? Name { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ser válido")]
        public String? Email { get; set; }

        public String? Password { get; set; }

        public String? OriginCity { get; set; }

        public String? Telephone { get; set; }

        public byte[]? Picture { get; set; }

        public String? CPF { get; set; }

        public String? AttendedCities { get; set; }

        public String? WhereToFind { get; set; }
    }
}
