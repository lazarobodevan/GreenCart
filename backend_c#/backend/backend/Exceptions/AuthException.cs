namespace backend.Exceptions {
    public class AuthException : Exception{
       
        public AuthException():base("Erro de autenticação") { }

        public AuthException(string message) : base(message) { }
    }
}
