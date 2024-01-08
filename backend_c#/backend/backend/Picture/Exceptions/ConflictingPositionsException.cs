using System;

namespace backend.Picture.Exceptions {
    public class ConflictingPositionsException : Exception {

        public ConflictingPositionsException(int position) :base($"Já existe uma imagem na posição {position}"){ }

        public ConflictingPositionsException(string message) : base(message) { }

    }
}
