using backend.ProducerPicture.DTOs;
using NetTopologySuite.Geometries;
using System;
using System.Text.Json.Serialization;

namespace backend.Producer.DTOs;

public class ListProducerDTO{
    public ListProducerDTO(Models.Producer producer, ListProducerPictureDTO? pictureDTO){
        Id = producer.Id;
        Name = producer.Name;
        Email = producer.Email;
        Telephone = producer.Telephone;
        Picture = producer.HasProfilePicture && pictureDTO != null ? new ListProducerPictureDTO() {
            ProducerId = producer.Id,
            Url = pictureDTO.Url
        } : null;
        Latitude = producer.Location.Y;
        Longitude = producer.Location.X;
        RatingsAvg = producer.RatingsAvg;
        RatingsCount = producer.RatingsCount;
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

    [JsonPropertyName("telephone")] public string Telephone{ get; set; }

    [JsonPropertyName("picture")] public ListProducerPictureDTO? Picture{ get; set; }

    [JsonPropertyName("whereToFind")] public string WhereToFind{ get; set; }

    [JsonPropertyName("latitude")] public double Latitude { get; set; }

    [JsonPropertyName("location")] public Point Location { get; set; }

    [JsonPropertyName("longitude")] public double Longitude { get; set; }

    [JsonPropertyName("ratingsAvg")] public double RatingsAvg { get; set; }

    [JsonPropertyName("ratingsCount")] public int RatingsCount { get; set; }

    [JsonPropertyName("createdAt")] public DateTime CreatedAt{ get; set; }

    [JsonPropertyName("updatedAt")] public DateTime UpdatedAt{ get; set; }

    [JsonPropertyName("deletedAt")] public DateTime? DeletedAt{ get; set; }
}