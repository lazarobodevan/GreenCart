using backend.Shared.Interfaces;
using System.Collections.Generic;

namespace backend.Product.DTOs {
    public class ListProductsPagination : IPagination {
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Offset { get; set; }
        public List<ListProductDTO> Products { get; set; }
    }
}
