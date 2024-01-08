using backend.Shared.Interfaces;
using System.Collections.Generic;

namespace backend.Product.DTOs {
    public class ListDatabaseProductsPagination: IPagination {
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Offset { get; set; }
        public List<Models.Product> Products { get; set; } = new List<Models.Product>();
        public string? NextUrl { get; set; }
        public string? PreviousUrl { get; set; }
    }
}
