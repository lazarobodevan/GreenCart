using System.Collections.Generic;
using backend.Models;
using backend.Producer.DTOs;
using backend.Shared.Classes;

namespace backend.Product.DTOs {
    public class ListProductsResponse {
        public ListProducerDTO? Producer { get; set; }
        public Pagination<ListProductDTO> Products { get; set; } = new Pagination<ListProductDTO>();
    }
}
