using System.ComponentModel.DataAnnotations;

namespace backend.Producer.Queries
{
    public class ProducerSearchNearbyQuery
    {
        [Required(ErrorMessage = "Latitude é obrigatória")]
        public double? Latitude { get; set; }

        [Required(ErrorMessage = "Longitude é obrigatória")]
        public double? Longitude { get; set; }

        [Required(ErrorMessage = "Raio em Km é obrigatório")]
        public int? RadiusInKm { get; set; }
    }
}
