namespace Projeto.Models
{
    public class Usuario
    {
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty; 
        public string Nome { get; set; } = string.Empty; 


        public int? TurmaId { get; set; } 
    }
}
