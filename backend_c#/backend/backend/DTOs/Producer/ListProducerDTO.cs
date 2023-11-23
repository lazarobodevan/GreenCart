using backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.DTOs.Producer {
    public class ListProducerDTO {

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public String Name { get; set; }

        [JsonPropertyName("email")]
        public String Email { get; set; }

        [JsonPropertyName("originCity")]
        public String OriginCity { get; set; }

        [JsonPropertyName("telephone")]
        public String Telephone { get; set; }

        [JsonPropertyName("picture")]
        public byte[]? Picture { get; set; }

        [JsonPropertyName("attendedCities")]
        public String AttendedCities { get; set; }

        [JsonPropertyName("whereToFind")]
        public String WhereToFind { get; set; }
        
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        public ListProducerDTO(Models.Producer producer) {
            Id = producer.Id;
            Name = producer.Name;
            Email = producer.Email;
            OriginCity = producer.OriginCity;
            Telephone = producer.Telephone;
            Picture = producer.Picture;
            AttendedCities = producer.AttendedCities;
            WhereToFind = producer.WhereToFind;
            CreatedAt = producer.CreatedAt;
            UpdatedAt = producer.UpdatedAt;
            DeletedAt = producer.DeletedAt;
        }

        public ListProducerDTO() { }
    }
}
