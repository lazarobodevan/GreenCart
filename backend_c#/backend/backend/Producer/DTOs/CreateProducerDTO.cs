using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using backend.Models;
using backend.ProducerPicture.DTOs;
using Microsoft.AspNetCore.Http;
using ThirdParty.Json.LitJson;

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

    public IFormFile? Picture{ get; set; }

    [Required(ErrorMessage = "Latitude é obrigatória")]
    public double? Latitude{ get; set; }

    [Required(ErrorMessage = "Longitude é obrigatória")]
    public double? Longitude{ get; set; }

    [Required(ErrorMessage ="Onde te encontrar é obrigatório")]
    public string WhereToFind { get; set; }
}