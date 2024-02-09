using System.ComponentModel.DataAnnotations;

namespace backend.Producer.DTOs {
    public class ProducerSearchParamentersDTO {
        [Required(ErrorMessage ="Latitude é obrigatória")]
        public double? Latitude { get; set; }

        [Required(ErrorMessage ="Longitude é obrigatória")]
        public double? Longitude { get; set;}

        [Required(ErrorMessage ="Raio em Km é obrigatório")]
        public int? RadiusInKm { get; set; }
    }
}
