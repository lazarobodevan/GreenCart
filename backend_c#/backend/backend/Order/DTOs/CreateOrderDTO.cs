using System;
using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.Order.DTOs {
    public class CreateOrderDTO {
        public Guid Id { get; set; }

        [Required]
        public Guid? ProductId { get; set; }

        [Required]
        public Guid? ConsumerId { get; set; }

        [Required]
        public int? Quantity { get; set; }

        public string? Observation { get; set; }

    }
}
