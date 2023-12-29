using System;

namespace backend.Picture.DTOs {
    public class ListPictureDTO {
        public string Url { get; set; }
        public Guid ProductId { get; set; }
        public int Position { get; set; }
    }
}
