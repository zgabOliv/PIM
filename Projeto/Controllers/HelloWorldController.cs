using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Projeto.Data;
using Projeto.Models;
using Projeto.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projeto.Controllers
{
    public class HelloWorldController : Controller
    {
        private readonly ServicoAutenticacao _authService;

        public HelloWorldController(ServicoAutenticacao authService)
        {
            _authService = authService;
        }

        // ------------------- LOGIN -------------------
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (TempData["MensagemSucesso"] != null)
            {
                ViewBag.MensagemSucesso = TempData["MensagemSucesso"];
            }

            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            if (_authService.Autenticar(email, senha))
            {
                var usuario = _authService.ObterUsuarioLogado(email);

                if (usuario == null ||
                    !(usuario.Perfil.Equals("aluno", StringComparison.OrdinalIgnoreCase) ||
                      usuario.Perfil.Equals("professor", StringComparison.OrdinalIgnoreCase)))
                {
                    ViewBag.Erro = "Perfil de usuário inválido ou não reconhecido. Contate o suporte.";
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Email),
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Perfil)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                // Redireciona para a dashboard correta
                if (usuario.Perfil.Equals("aluno", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("DashboardAluno", "HelloWorld");
                }
                else
                {
                    return RedirectToAction("DashboardProfessor", "HelloWorld");
                }
            }
            else
            {
                ViewBag.Erro = "E-mail ou senha incorretos!";
                return View();
            }

        }

        // ------------------- CADASTRO -------------------
        [HttpGet]
   

        public IActionResult Cadastro()
        {
            var repoTurmas = new RepositorioTurmasJson();
            ViewBag.Turmas = repoTurmas.Listar();
            return View();
        }
        [HttpPost]
        [HttpPost]
        public IActionResult Cadastro(string nome, string email, string senha, string perfil, int? turmaId)
        {
            var repoTurmas = new RepositorioTurmasJson();
            ViewBag.Turmas = repoTurmas.Listar();

            try
            {
                if (perfil.Equals("aluno", StringComparison.OrdinalIgnoreCase) && turmaId.HasValue)
                {
                    _authService.RegistrarNovoUsuario(email, senha, perfil, turmaId.Value, nome);
                }
                else
                {
                    _authService.RegistrarNovoUsuario(email, senha, perfil, null, nome);
                }

                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso! Faça login.";
                return RedirectToAction("Login");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.Erro = ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Erro = "Erro interno ao processar o cadastro: " + ex.Message;
                return View();
            }
        }




        // ------------------- DASHBOARDS -------------------
        [HttpGet]
        public IActionResult DashboardAluno()
        {
            // 1. Obter o usuário logado
            var emailUsuario = User.FindFirst(ClaimTypes.Name)?.Value;
            var usuario = _authService.ObterUsuarioLogado(emailUsuario);

            if (usuario == null) return RedirectToAction("Login");

            // 2. Buscar a turma do aluno
            var turmasRepo = new RepositorioTurmasJson();
            var turmaAluno = turmasRepo.Listar().FirstOrDefault(t => t.Id == usuario.TurmaId);

            // 3. Buscar atividades da turma do aluno
            var atividadesRepo = new RepositorioAtividades();
            var atividadesAluno = turmaAluno != null
                ? atividadesRepo.Carregar().Where(a => a.TurmaId == turmaAluno.Id).ToList()
                : new List<Atividade>();

            ViewBag.Turma = turmaAluno;
            ViewBag.Atividades = atividadesAluno;
            return View(usuario);

        }


        [HttpGet]
        public IActionResult DashboardProfessor()
        {
            return View();
        }
        public IActionResult Perguntar(string pergunta)
        {
            var resposta = ChatbotService.ObterResposta(pergunta);
            return Json(new { resposta });
        }


        // ------------------- LOGOUT -------------------//
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
