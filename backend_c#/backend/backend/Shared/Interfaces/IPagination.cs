using System.Collections.Generic;

namespace backend.Shared.Interfaces {
    public interface IPagination {
        int Pages { get; set; }
        int CurrentPage { get; set; }
        public string? NextUrl { get; set; }
        public string? PreviousUrl { get; set; }
    }
}
