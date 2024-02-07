using System;

namespace backend.Picture.Exceptions {
    public class ExceededNumberOfPicturesException : Exception {
        public ExceededNumberOfPicturesException() : base("Número de imagens cadastradas excedido") { }

        public ExceededNumberOfPicturesException(string message) : base(message) { }
    }
}
