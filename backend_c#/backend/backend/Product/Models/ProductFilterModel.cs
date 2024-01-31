using backend.Product.Enums;
using System;

namespace backend.Product.Models {
    public class ProductFilterModel {
        public string? Name { get; set; }
        public Category? Category { get; set; }
        public bool? IsByPrice { get; set; }
        public Guid? ProducerId { get; set; }
        public bool? IsOrganic { get; set; }
    }
}
