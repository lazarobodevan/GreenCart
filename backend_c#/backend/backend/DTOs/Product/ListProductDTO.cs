using backend.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Product {
    public class ListProductDTO {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public byte[]? Picture { get; set; }

        public Category Category { get; set; }

        public double Price { get; set; }

        public Unit Unit { get; set; }

        public int AvailableQuantity { get; set; }

        public bool IsOrganic { get; set; }

        public DateTime HarvestDate { get; set; }

        public Models.Producer Producer { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

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
    }
}
