using System;

namespace Projeto.Models
{
    public class Atividade
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int TurmaId { get; set; } // referência à turma
        public DateTime DataEntrega { get; set; }
    }
}
