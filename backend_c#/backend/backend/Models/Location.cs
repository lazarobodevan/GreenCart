using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models {
    [Table("Locations")]
    public class Location {
        [Key]
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [Column("Address")]
        public string Address { get; set; }

        [Required]
        [Column("City")]
        public string City { get; set; }

        [Required]
        [Column("State")]
        public string State {  get; set; }

        [Required]
        [Column("ZipCode")]
        public string ZipCode { get; set; }
    }
}
