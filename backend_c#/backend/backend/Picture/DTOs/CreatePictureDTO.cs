using System;
using System.IO;

namespace backend.Picture.DTOs
{
    public class CreatePictureDTO
    {

        public Guid Key { get; set; }
        public int Position { get; set; }
        public Stream Stream { get; set; }
    }
}
