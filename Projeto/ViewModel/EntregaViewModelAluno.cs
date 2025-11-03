namespace Projeto.ViewModels
{
    public class EntregaViewModelAluno
    {
        public int Id { get; set; }
        public string TituloAtividade { get; set; } = string.Empty;
        public double? Nota { get; set; }
        public string FeedbackProfessor { get; set; } = string.Empty;
    }
}
