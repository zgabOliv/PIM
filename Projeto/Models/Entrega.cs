namespace Projeto.Models
{
    public class Entrega
    {
        public int Id { get; set; }
        public int AtividadeId { get; set; }
        public string NomeAluno { get; set; } = string.Empty; 
        public string RespostaAluno { get; set; } = string.Empty;
        public double? Nota { get; set; }
        public string FeedbackProfessor { get; set; } = string.Empty;


        public string TituloAtividade { get; set; } = string.Empty;

        public int? TurmaId { get; set; }
        public string? CaminhoArquivo { get; set; }
    }
}
