using backend.Picture.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json.Serialization;

namespace backend.ProductPicture.DTOs {
    public class UpdateProductPicturesDTO {

        public List<IFormFile>? Pictures { get; set; }

        [Required(ErrorMessage = "Metadados são obrigatórios")]
        public List<ProductPictureRequestDTO>? PicturesMetadata { get; set; }
    }
}
