using System.ComponentModel.DataAnnotations;
using System.IO;
using System;

namespace backend.ProducerPicture.DTOs
{
    public class CreateProducerPictureDTO{

        [Key]
        public Guid? Key { get; set; }

        [Required]
        public Stream? Stream { get; set; }
    }
}
