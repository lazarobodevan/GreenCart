using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Producer.DTOs;

public class UpdateProducerDTO{
    public Guid Id{ get; set; }

    public string? Name{ get; set; }

    [EmailAddress(ErrorMessage = "Email deve ser válido")]
    public string? Email{ get; set; }

    public string? Password{ get; set; }

    public string? OriginCity{ get; set; }

    public string? Telephone{ get; set; }

    public byte[]? Picture{ get; set; }

    public string? CPF{ get; set; }

    public string? AttendedCities{ get; set; }

    public string? WhereToFind{ get; set; }
}