using System;
using System.Text.Json.Serialization;

namespace backend.Producer.DTOs;

public class ListProducerDTO{
    public ListProducerDTO(Models.Producer producer){
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

    public ListProducerDTO(){
    }

    [JsonPropertyName("id")] public Guid Id{ get; set; }

    [JsonPropertyName("name")] public string Name{ get; set; }

    [JsonPropertyName("email")] public string Email{ get; set; }

    [JsonPropertyName("originCity")] public string OriginCity{ get; set; }

    [JsonPropertyName("telephone")] public string Telephone{ get; set; }

    [JsonPropertyName("picture")] public byte[]? Picture{ get; set; }

    [JsonPropertyName("attendedCities")] public string AttendedCities{ get; set; }

    [JsonPropertyName("whereToFind")] public string WhereToFind{ get; set; }

    [JsonPropertyName("createdAt")] public DateTime CreatedAt{ get; set; }

    [JsonPropertyName("updatedAt")] public DateTime UpdatedAt{ get; set; }

    [JsonPropertyName("deletedAt")] public DateTime? DeletedAt{ get; set; }
}