namespace Projeto.ViewModels
{
    public class EntregaCorrigirViewModel
    {
        public int Id { get; set; } // Id da entrega
        public int AtividadeId { get; set; } 
        public int entrega { get; set; } 
        public string NomeAluno { get; set; }
        public string RespostaAluno { get; set; }
        public double? Nota { get; set; }
        public string FeedbackProfessor { get; set; }


        public string? CaminhoArquivo { get; set; }
    }
}
