using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace backend.Picture.DTOs
{
    public class CreateProductPictureDTO
    {
        public Guid? Key { get; set; }

        [Required]
        public int? Position { get; set; }

        [Required]
        public IFormFile? Stream { get; set; }
    }
}
