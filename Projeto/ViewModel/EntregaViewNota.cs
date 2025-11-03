namespace Projeto.ViewModels
{
    public class EntregaViewNota
    {
        public int AtividadeId { get; set; }
        public string RespostaAluno { get; set; }
        public double? Nota { get; set; } // Nullable caso ainda não tenha sido corrigida
        public string FeedbackProfessor { get; set; }
    }
}
