using System;

namespace backend.Order.Exceptions {
    public class OrderDoesNotExistException: Exception {
        public OrderDoesNotExistException() : base("Encomenda não existe"){ }
        public OrderDoesNotExistException(string message) : base(message) { }
    }
}
