namespace Projeto.Models
{
    public class Turma
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Curso { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public int Ano { get; set; }
        public int? Professor { get; set; } = 0;

    }
}