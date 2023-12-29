using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using backend.Picture.DTOs;
using backend.Product.Enums;
using Microsoft.AspNetCore.Http;

namespace backend.Product.DTOs;

public class CreateProductDTO{
    public CreateProductDTO(){
    }

    public CreateProductDTO(string name, string? description, List<IFormFile>? picture, Category? category, double? price,
        Unit? unit, int? availableQuantity, bool? isOrganic, string? harvestDate, Guid? producerId){
        Name = name;
        Description = description;
        Pictures = picture;
        Category = category;
        Price = price;
        Unit = unit;
        AvailableQuantity = availableQuantity;
        IsOrganic = isOrganic;
        HarvestDate = harvestDate;
        ProducerId = producerId;
    }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [MinLength(3)]
    public string Name{ get; set; }

    [Required(ErrorMessage = "Descrição é obrigatória")]
    public string? Description{ get; set; }

    [Required(ErrorMessage = "Foto é obrigatória")]
    public List<IFormFile>? Pictures{ get; set; }

    [Required(ErrorMessage ="Metadados são obrigatórios")]
    public List<PictureRequestDTO>? PicturesMetadata { get; set; }

    [Required(ErrorMessage = "Categoria é obrigatório")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Category? Category{ get; set; }

    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0, double.MaxValue)]
    public double? Price{ get; set; }

    [Required(ErrorMessage = "Unidade é obrigatória")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Unit? Unit{ get; set; }

    [Required(ErrorMessage = "Quantidade disponível é obrigatória")]
    public int? AvailableQuantity{ get; set; }

    [Required(ErrorMessage = "É Orgânico? é obrigatório")]
    public bool? IsOrganic{ get; set; }

    [Required(ErrorMessage = "Data da colheita/produção é obrigatória")]
    public string? HarvestDate{ get; set; }

    [Required(ErrorMessage = "Id do produtor é obrigatório")]
    public Guid? ProducerId{ get; set; }
}