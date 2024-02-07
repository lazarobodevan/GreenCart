using System.Collections.Generic;

namespace backend.Shared.Classes {
    public class Pagination<T>{
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int Offset { get; set; }
        public string? NextUrl { get; set; }
        public string? PreviousUrl { get; set; }
        public List<T> Data { get; set; } = new List<T>();

    }
}
