using System;

namespace backend.Product.Exceptions {
    public class ProductDoesNotExistException : Exception {

        public ProductDoesNotExistException():base("Produto não existe") { }

        public ProductDoesNotExistException(string message) : base(message) {
        }
    }
}
