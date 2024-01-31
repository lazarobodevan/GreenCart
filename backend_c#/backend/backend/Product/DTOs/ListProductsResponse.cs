using System.Collections.Generic;
using backend.Models;
using backend.Producer.DTOs;
using backend.Shared.Interfaces;
namespace backend.Product.DTOs {
    public class ListProductsResponse: IPagination {
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public string? NextUrl { get; set; }
        public string? PreviousUrl { get; set; }
        public ListProducerDTO? Producer { get; set; }
        public List<ListProductDTO> Products { get; set; } = new List<ListProductDTO>();
    }
}
