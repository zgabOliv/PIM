using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Projeto.Models;

namespace Projeto.Repositories
{
    public class RepositorioTokensJson
    {
        private readonly string _path;

        public RepositorioTokensJson()
        {
            var basePath = AppContext.BaseDirectory;
            var projectRoot = Directory.GetParent(basePath).Parent.Parent.FullName; // ajusta se necessário
            var dataDir = Path.Combine(projectRoot, "Data");
            Directory.CreateDirectory(dataDir);
            _path = Path.Combine(dataDir, "tokens.json");
            if (!File.Exists(_path)) File.WriteAllText(_path, "[]");
        }

        private List<TokenRecuperacao> LerTodos()
        {
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<TokenRecuperacao>>(json) ?? new List<TokenRecuperacao>();
        }

        private void SalvarTodos(List<TokenRecuperacao> lista)
        {
            var json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_path, json);
        }

        public void Salvar(TokenRecuperacao token)
        {
            var lista = LerTodos();
            lista.Add(token);
            SalvarTodos(lista);
        }

        public TokenRecuperacao Buscar(string token)
        {
            var lista = LerTodos();
            return lista.FirstOrDefault(t => t.Token == token);
        }

        public void Remover(string token)
        {
            var lista = LerTodos();
            lista = lista.Where(t => t.Token != token).ToList();
            SalvarTodos(lista);
        }

        public void RemoverExpirados()
        {
            var lista = LerTodos();
            lista = lista.Where(t => t.ExpiraEm > DateTime.Now).ToList();
            SalvarTodos(lista);
        }
    }
}
