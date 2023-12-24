using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using backend.Product.Enums;

namespace backend.Product.DTOs;

public class UpdateProductDTO{
    public Guid Id{ get; set; }

    [MinLength(3)] public string? Name{ get; set; }

    public string? Description{ get; set; }

    public byte[]? Picture{ get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Category? Category{ get; set; }

    [Range(0, double.MaxValue)] public double? Price{ get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Unit? Unit{ get; set; }

    public int? AvailableQuantity{ get; set; }

    public bool? IsOrganic{ get; set; }

    public string? HarvestDate{ get; set; }

    private DateTime? UpdatedAt{ get; set; }
}