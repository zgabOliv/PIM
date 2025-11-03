
using Projeto.Models;
using System.Text.Json;

public class RepositorioAtividades
{
    private readonly string _caminhoArquivo = "Data/atividades.json";

    public List<Atividade> Carregar()
    {
        if (!File.Exists(_caminhoArquivo)) return new List<Atividade>();
        var json = File.ReadAllText(_caminhoArquivo);
        return JsonSerializer.Deserialize<List<Atividade>>(json) ?? new List<Atividade>();
    }

    public void Salvar(List<Atividade> atividades)
    {
        var json = JsonSerializer.Serialize(atividades, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_caminhoArquivo, json);
    }
}
