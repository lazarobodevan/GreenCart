using backend.Enums;
using backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Product {
    public class CreateProductDTO {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public byte[]? Picture { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public Unit Unit { get; set; }

        [Required]
        public int AvailableQuantity { get; set; }

        [Required]
        public bool IsOrganic { get; set; }

        [Required]      
        public DateTime HarvestDate { get; set; }

        [Required]
        public Guid ProducerId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public CreateProductDTO(string name, string? description, byte[]? picture, Category category, double price, Unit unit, int availableQuantity, bool isOrganic, DateTime harvestDate, Guid producerId) {
            Name = name;
            Description = description;
            Picture = picture;
            Category = category;
            Price = price;
            Unit = unit;
            AvailableQuantity = availableQuantity;
            IsOrganic = isOrganic;
            HarvestDate = harvestDate;
            ProducerId = producerId;
            CreatedAt = new DateTime();
            UpdatedAt = CreatedAt;
        }
    }
}
