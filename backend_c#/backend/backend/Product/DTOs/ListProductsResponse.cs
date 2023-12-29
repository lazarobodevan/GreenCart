using System.Collections.Generic;
using backend.Models;
namespace backend.Product.DTOs {
    public class ListProductsResponse {
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public string? NextUrl { get; set; }
        public string? PreviousUrl { get; set; }
        public List<ListProductDTO> Products { get; set; } = new List<ListProductDTO>();
    }
}
