using backend.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.DTOs.Product {
    public class ListProductDTO {

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("picture")]
        public byte[]? Picture { get; set; }

        [JsonPropertyName("category")]
        public Category Category { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("unit")]
        public Unit Unit { get; set; }

        [JsonPropertyName("availableQuantity")]
        public int AvailableQuantity { get; set; }

        [JsonPropertyName("isOrganic")]
        public bool IsOrganic { get; set; }

        [JsonPropertyName("harvestDate")]
        public DateTime HarvestDate { get; set; }

        [JsonPropertyName("producer")]
        public Models.Producer Producer { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        public ListProductDTO(Models.Product product) {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Picture = product.Picture;
            Category = product.Category;
            Price = product.Price;
            Unit = product.Unit;
            AvailableQuantity = product.AvailableQuantity;
            IsOrganic = product.IsOrganic;
            HarvestDate = product.HarvestDate;
            Producer = product.Producer;
            CreatedAt = product.CreatedAt;
            UpdatedAt = product.UpdatedAt;
            DeletedAt = product.DeletedAt;
        }

        public ListProductDTO() { }
    }
}
