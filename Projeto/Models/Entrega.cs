namespace Projeto.Models
{
    public class Entrega
    {
        public int Id { get; set; }
        public int AtividadeId { get; set; }
        public string NomeAluno { get; set; } = string.Empty; // ou Email se preferir
        public string RespostaAluno { get; set; } = string.Empty;
        public double? Nota { get; set; }
        public string FeedbackProfessor { get; set; } = string.Empty;

        // opcional: você pode armazenar o título da atividade pra facilitar a view de notas
        public string TituloAtividade { get; set; } = string.Empty;
        // opcional: TurmaId caso precise filtrar por turma
        public int? TurmaId { get; set; }
    }
}
