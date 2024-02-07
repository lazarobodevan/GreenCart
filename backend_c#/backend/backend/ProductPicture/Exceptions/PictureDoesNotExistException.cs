using System;

namespace backend.Picture.Exceptions {
    public class PictureDoesNotExistException : Exception{
        public PictureDoesNotExistException():base("Imagem não existe") { }

        public PictureDoesNotExistException(string message) : base(message) { }
    }
}
