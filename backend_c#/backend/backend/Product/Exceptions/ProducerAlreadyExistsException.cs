using System;

namespace backend.Product.Exceptions
{
    public class ProducerAlreadyExistsException : Exception
    {
        public ProducerAlreadyExistsException() : base("Produtor já existe") { }

        public ProducerAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
