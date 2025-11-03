namespace Projeto.Models
{
    public class Usuario
    {
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty; // "aluno" ou "professor"
        public string Nome { get; set; } = string.Empty; // "aluno" ou "professor"

        // Nova propriedade
        public int? TurmaId { get; set; } // O aluno está matriculado em qual turma
    }
}
