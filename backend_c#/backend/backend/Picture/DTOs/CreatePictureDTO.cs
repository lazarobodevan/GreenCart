using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace backend.Picture.DTOs
{
    public class CreatePictureDTO
    {
        public Guid? Key { get; set; }

        [Required]
        public int? Position { get; set; }

        [Required]
        public Stream? Stream { get; set; }
    }
}
