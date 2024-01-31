using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Product.Enums;

namespace backend.Models;

[Table("Products")]
public class Product{
    public Product(){
    }


    [Key] [Column("Id")] public Guid Id{ get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [Column("Name")]
    public string Name{ get; set; }

    [Column("NormalizedName")]
    public string NormalizedName { get; set; }

    [Column("Description")] public string? Description{ get; set; }

    public List<Picture> Pictures { get; set; }

    [Required(ErrorMessage = "Categoria é obrigatório")]
    [Column("Category")]
    public Category Category{ get; set; }

    [Required(ErrorMessage = "Preço é obrigatório")]
    [Column("Price")]
    public double Price{ get; set; }

    [Required(ErrorMessage = "Unidade é obrigatório")]
    [Column("Unit")]
    public Unit Unit{ get; set; }

    [Required(ErrorMessage = "Quantidade disponível é obrigatório")]
    [Column("AvailableQuantity")]
    public int AvailableQuantity{ get; set; }

    [Required(ErrorMessage = "É Orgânico é obrigatório")]
    [Column("IsOrganic")]
    public bool IsOrganic{ get; set; }

    [Required(ErrorMessage = "Data da colheita/produção é obrigatório")]
    [Column("HarvestDate")]
    public DateTime HarvestDate{ get; set; }

    [ForeignKey("Producer")] public Guid ProducerId{ get; set; }

    public Producer Producer{ get; set; }

    [Column("CreatedAt")] public DateTime CreatedAt{ get; set; }

    [Column("UpdatedAt")] public DateTime UpdatedAt{ get; set; }

    [Column("DeletedAt")] public DateTime? DeletedAt{ get; set; }
}