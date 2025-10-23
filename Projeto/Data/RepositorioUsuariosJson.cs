using Projeto.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Text.Json;
using System; 


namespace Projeto.Data
{
    public class RepositorioUsuariosJson
    {
        
        private readonly string _caminhoArquivo = "Data/usuarios.json";

        

        private List<Usuario> CarregarUsuarios()
        {
            
            if (!File.Exists(_caminhoArquivo))
            {
                
                return new List<Usuario>();
            }

         
            string json = File.ReadAllText(_caminhoArquivo);

            
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

 
        private void SalvarUsuarios(List<Usuario> usuarios)
        {
      
            string diretorio = Path.GetDirectoryName(_caminhoArquivo)!;

            if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

       
            var options = new JsonSerializerOptions
            {
                WriteIndented = true 
            };
            string json = JsonSerializer.Serialize(usuarios, options);

           
            File.WriteAllText(_caminhoArquivo, json);
        }

      
        public Usuario EncontrarPorEmail(string email)
        {
            var usuarios = CarregarUsuarios();

          
            return usuarios.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

     
        public void Adicionar(Usuario novoUsuario)
        {
            var usuarios = CarregarUsuarios();

        
            if (usuarios.Any(u => u.Email.Equals(novoUsuario.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Email já cadastrado. Não é possível adicionar.");
            }

            usuarios.Add(novoUsuario);
            SalvarUsuarios(usuarios);
        }
    }
}