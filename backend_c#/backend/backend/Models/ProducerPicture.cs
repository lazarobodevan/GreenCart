using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models {
    [Table(name:"ProducerPicture")]
    public class ProducerPicture {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Producer")]
        public Guid ProducerId { get; set; }
    }
}
