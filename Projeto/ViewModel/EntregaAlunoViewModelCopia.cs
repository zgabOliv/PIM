using Microsoft.AspNetCore.Http; 

namespace Projeto.ViewModels
{
    public class EntregaAlunoViewModel
    {
        public int AtividadeId { get; set; }
        public string TituloAtividade { get; set; } = string.Empty;
        public string RespostaAluno { get; set; } = string.Empty;

        // Campo para o upload do arquivo
        public IFormFile? ArquivoAluno { get; set; }

        // NOVO campo para guardar o caminho do arquivo
        public string? CaminhoArquivo { get; set; }
    }
}
