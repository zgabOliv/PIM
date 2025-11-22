using Projeto.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Projeto.Data
{
    public class RepositorioTurmasJson
    {
        private readonly string _caminhoArquivo = "Data/turmas.json";

        public List<Turma> CarregarTurmas()
        {
            if (!File.Exists(_caminhoArquivo))
                return new List<Turma>();

            string json = File.ReadAllText(_caminhoArquivo);
            return JsonSerializer.Deserialize<List<Turma>>(json) ?? new List<Turma>();
        }

        public void SalvarTurmas(List<Turma> turmas)
        {
            var diretorio = Path.GetDirectoryName(_caminhoArquivo);
            if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
                Directory.CreateDirectory(diretorio);

            string json = JsonSerializer.Serialize(turmas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_caminhoArquivo, json);
        }

        public List<Turma> Listar()
        {
            return CarregarTurmas();
        }
        public void Adicionar(Turma novaTurma)
        {
            var turmas = CarregarTurmas();

            novaTurma.Id = turmas.Any()
                ? turmas.Max(t => t.Id) + 1
                : 1;

            turmas.Add(novaTurma);
            SalvarTurmas(turmas);
        }
    }
}
