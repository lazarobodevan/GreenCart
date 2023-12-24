using System;

namespace backend.Product.Exceptions
{
    public class ProducerDoesNotExistException : Exception
    {
        public ProducerDoesNotExistException() : base("Produtor não existe") { }

        public ProducerDoesNotExistException(string message) : base(message)
        {
        }
    }
}
