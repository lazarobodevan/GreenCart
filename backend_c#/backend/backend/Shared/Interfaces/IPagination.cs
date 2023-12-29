using System.Collections.Generic;

namespace backend.Shared.Interfaces {
    public interface IPagination {
        int Pages { get; set; }
        int CurrentPage { get; set; }
        int Offset { get; set; }
        
    }
}
