using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using backend.Picture.DTOs;
using backend.Producer.DTOs;
using backend.Product.Enums;

namespace backend.Product.DTOs;

public class ListProductDTO{
    public ListProductDTO(Models.Product product, List<ListPictureDTO> productPicturesUrls){
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        PicturesUrls = productPicturesUrls;
        Category = product.Category;
        Price = product.Price;
        Unit = product.Unit;
        AvailableQuantity = product.AvailableQuantity;
        IsOrganic = product.IsOrganic;
        HarvestDate = product.HarvestDate;
        Producer = product.Producer != null ? new ListProducerDTO() {
            Id = product.Producer.Id,
            Name = product.Producer.Name,
            Picture = product.Producer.Picture,
            Email = product.Producer.Email,
            OriginCity = product.Producer.OriginCity,
            AttendedCities = product.Producer.AttendedCities,
            WhereToFind = product.Producer.WhereToFind,
        }: null;
        CreatedAt = product.CreatedAt;
        UpdatedAt = product.UpdatedAt;
        DeletedAt = product.DeletedAt;
    }

    public ListProductDTO(){
    }

    [JsonPropertyName("id")] public Guid Id{ get; set; }

    [JsonPropertyName("name")] public string Name{ get; set; }

    [JsonPropertyName("description")] public string? Description{ get; set; }

    [JsonPropertyName("picture")] public List<ListPictureDTO> PicturesUrls{ get; set; }

    [JsonPropertyName("category")] public Category Category{ get; set; }

    [JsonPropertyName("price")] public double Price{ get; set; }

    [JsonPropertyName("unit")] public Unit Unit{ get; set; }

    [JsonPropertyName("availableQuantity")]
    public int AvailableQuantity{ get; set; }

    [JsonPropertyName("isOrganic")] public bool IsOrganic{ get; set; }

    [JsonPropertyName("harvestDate")] public DateTime HarvestDate{ get; set; }

    [JsonPropertyName("producer")] public ListProducerDTO? Producer{ get; set; }

    [JsonPropertyName("createdAt")] public DateTime CreatedAt{ get; set; }

    [JsonPropertyName("updatedAt")] public DateTime UpdatedAt{ get; set; }

    [JsonPropertyName("deletedAt")] public DateTime? DeletedAt{ get; set; }
}