namespace Projeto.Models
{
    public class Atividade
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Turma { get; set; } = string.Empty; // ou id da turma
        public DateTime DataEntrega { get; set; }
    }
}