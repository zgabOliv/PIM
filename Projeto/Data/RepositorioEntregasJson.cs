using Projeto.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace Projeto.Data
{
    public class RepositorioEntregasJson
    {
        private readonly string _caminhoArquivo = "Data/entregas.json";

        public List<Entrega> Carregar()
        {
            if (!File.Exists(_caminhoArquivo)) return new List<Entrega>();
            string json = File.ReadAllText(_caminhoArquivo);
            return JsonSerializer.Deserialize<List<Entrega>>(json) ?? new List<Entrega>();
        }
        public void Salvar(List<Entrega> entregas)
        {
            // Garante que a pasta Data exista
            var diretorio = Path.GetDirectoryName(_caminhoArquivo);
            if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
                Directory.CreateDirectory(diretorio);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(entregas, options);
            File.WriteAllText(_caminhoArquivo, json); // cria o arquivo se não existir
        }
    }
}