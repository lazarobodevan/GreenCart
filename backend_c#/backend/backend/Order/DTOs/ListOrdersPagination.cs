using backend.Shared.Interfaces;
using System.Collections.Generic;

namespace backend.Order.DTOs {
    public class ListOrdersPagination : IPagination {
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public string? NextUrl { get; set; }
        public string? PreviousUrl { get; set; }
        public List<Models.Order> Orders { get; set; }
    }
}
