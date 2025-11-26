using System.Text.Json;

namespace Projeto.Data
{
    public class RepositorioLogsJson
    {
        private readonly string _caminho = "Data/logs.json";

        public RepositorioLogsJson()
        {
            if (!File.Exists(_caminho))
                File.WriteAllText(_caminho, "[]");
        }

        public void Registrar(LogAcesso log)
        {
            var lista = Listar();
            lista.Add(log);

            File.WriteAllText(_caminho,
                JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true }));
        }

        public List<LogAcesso> Listar()
        {
            var arquivo = File.ReadAllText(_caminho);
            return JsonSerializer.Deserialize<List<LogAcesso>>(arquivo) ?? new List<LogAcesso>();
        }
    }

public class LogAcesso
{
    public int UsuarioId { get; set; }
    public DateTime DataHora { get; set; }
    public string Acao { get; set; }
}

}
