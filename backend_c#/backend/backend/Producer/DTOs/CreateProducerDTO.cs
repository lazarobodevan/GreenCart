using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Models;

namespace backend.Producer.DTOs;

public class CreateProducerDTO{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Name{ get; set; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ser válido")]
    public string Email{ get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Password{ get; set; }

    [Required(ErrorMessage = "Cidade de origem é obrigatório")]
    public string OriginCity{ get; set; }

    [Required(ErrorMessage = "Celular é obrigatório")]
    public string Telephone{ get; set; }

    [Column("Picture")] public byte[]? Picture{ get; set; }

    [Required(ErrorMessage = "CPF é obrigatório")]
    public string CPF{ get; set; }

    [Required(ErrorMessage = "Cidades atendidas é obrigatório")]
    public string AttendedCities{ get; set; }

    [Required(ErrorMessage = "Onde te encontrar é obrigatório")]
    public string WhereToFind{ get; set; }

    public List<Models.Product>? Products{ get; set; }
    public List<Order>? Orders{ get; set; }
    public List<ConsumerFavProducer>? FavdByConsumers{ get; set; }
}