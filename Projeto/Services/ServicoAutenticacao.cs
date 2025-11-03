using BCrypt.Net;
using Projeto.Models;
using System;
using Projeto.Data;
using System.CodeDom.Compiler;

namespace Projeto.Services 
{
    public class ServicoAutenticacao
    {
       
        private readonly RepositorioUsuariosJson _repo;

        public ServicoAutenticacao(RepositorioUsuariosJson repo)
        {
            _repo = repo;
        }

        // =======================================================
        // LÓGICA DE CADASTRO (SALVAR NO JSON)
        // =======================================================

        /// <summary>
        /// Registra um novo usuário, criando o Hash seguro da senha antes de salvar.
        /// </summary>
        /// <param name="email">O email do usuário (usado como identificador).</param>
        /// <param name="senha">A senha em texto plano (do formulário).</param>
        public void RegistrarNovoUsuario(string email, string senha, string perfil, int? turmaId = null, string nome = "")
        {
            // 1. Gera o Hash da senha usando BCrypt.
            string senhaHash = BCrypt.Net.BCrypt.HashPassword(senha, workFactor: 12);

            var novoUsuario = new Usuario
            {
                Email = email, // Usa o campo Email
                SenhaHash = senhaHash, // Usa o campo SenhaHash
                Perfil = perfil, // Define um perfil padrão (ajuste se necessário)
                TurmaId = turmaId
            };

            // 2. Chama o Repositório para salvar o novo usuário no arquivo JSON
            _repo.Adicionar(novoUsuario);
        }

        public bool Autenticar(string emailDigitado, string senhaDigitada)
        {
            // 1. Busca o usuário no repositório (usando o método EncontrarPorEmail)
            var usuario = _repo.EncontrarPorEmail(emailDigitado);

            // Se o usuário não existe, falha na autenticação
            if (usuario == null)
            {
                return false;
            }

            // 2. Verifica a senha
            // BCrypt.Verify compara a senha digitada com o Hash armazenado (usuario.SenhaHash).
            bool senhaCorreta = BCrypt.Net.BCrypt.Verify(senhaDigitada, usuario.SenhaHash);

            return senhaCorreta;
        }

        // Opcional: Para obter o objeto usuário logado (útil para sessões)
        public Usuario ObterUsuarioLogado(string emailUsuario)
        {
            return _repo.EncontrarPorEmail(emailUsuario);
        }
    }
}