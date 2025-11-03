namespace Projeto.ViewModels
{
    public class EntregaCorrigirViewModel
    {
        public int Id { get; set; } // ID da entrega
        public int AtividadeId { get; set; } // ID da atividade relacionada
        public int entrega { get; set; } // ID da atividade relacionada
        public string NomeAluno { get; set; }
        public string RespostaAluno { get; set; }
        public double? Nota { get; set; }
        public string FeedbackProfessor { get; set; }
    }
}
