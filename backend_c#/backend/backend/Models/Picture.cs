using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models {
    [Table("Pictures")]
    public class Picture {
       
        [Column("Key")]
        [Key]
        public Guid Key { get; set; }

        [Column("Position")]
        public int Position { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

    }
}
