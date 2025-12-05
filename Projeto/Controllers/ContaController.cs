using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Projeto.Data;
using Projeto.Models;
using Projeto.Repositories;
using Projeto.Services;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Projeto.Controllers
{
    public class ContaController : Controller
    {
        private readonly RepositorioTokensJson _repoTokens = new();
        private readonly RepositorioUsuariosJson _repoUsuarios = new();
        private readonly EmailService _emailService;

        public ContaController(EmailService emailService)
        {
            _emailService = emailService;
        }



        [HttpGet]
        public IActionResult EsqueceuSenha()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnviarToken(string email)
        {
            if (string.IsNullOrEmpty(email)) return View("EsqueceuSenha");

            var usuario = _repoUsuarios.EncontrarPorEmail(email); 
            // não fala que o email não existe — evita enumerar usuários
            if (usuario == null)
            {
                return View("EmailEnviado");
            }

            // gerar token seguro (base64url)
            var bytes = new byte[48];
            RandomNumberGenerator.Fill(bytes);
            var token = Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');

            var tokenObj = new TokenRecuperacao
            {
                Token = token,
                UsuarioId = usuario.Id.ToString(),
                ExpiraEm = DateTime.Now.AddMinutes(30)
            };

            _repoTokens.Salvar(tokenObj);

            var link = Url.Action("Resetar", "Conta", new { token = token }, Request.Scheme);

            var assunto = "Recuperação de senha";
            var corpo = $"<p>Recebemos um pedido de recuperação de senha. Clique no link abaixo para resetar sua senha (válido por 30 minutos):</p>" +
                        $"<p><a href=\"{link}\">{link}</a></p>" +
                        "<p>Se você não solicitou, ignore este e-mail.</p>";

            try
            {
                _emailService.Enviar(usuario.Email, assunto, corpo);
            }
            catch (Exception)
            {
                // opcional: logar o erro
            }

            return View("EmailEnviado");
        }

        [HttpGet]
        public IActionResult Resetar(string token)
        {
            if (string.IsNullOrEmpty(token)) return View("TokenInvalido");

            _repoTokens.RemoverExpirados();
            var dados = _repoTokens.Buscar(token);
            if (dados == null || dados.ExpiraEm < DateTime.Now)
                return View("TokenInvalido");

            var vm = new ResetarSenhaViewModel { Token = token };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Resetar(ResetarSenhaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _repoTokens.RemoverExpirados();
            var dados = _repoTokens.Buscar(model.Token);
            if (dados == null || dados.ExpiraEm < DateTime.Now)
                return View("TokenInvalido");

            var usuarioId = int.Parse(dados.UsuarioId);
            var usuario = _repoUsuarios.BuscarPorId(usuarioId);


            // ajuste
            if (usuario == null) return View("TokenInvalido");

            // hash da senha com BCrypt
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(model.NovaSenha);

            _repoUsuarios.Salvar(usuario);

            _repoTokens.Remover(model.Token);

            return View("SenhaRedefinida");
        }
    }
}
