using System;

namespace Projeto.Models
{
    public class TokenRecuperacao
    {
        public string Token { get; set; }
        public string UsuarioId { get; set; }
        public DateTime ExpiraEm { get; set; }
    }
}
