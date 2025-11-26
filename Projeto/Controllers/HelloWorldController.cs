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
                ViewBag.MensagemSucesso = TempData["MensagemSucesso"];

            if (User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var usuario = _authService.ObterUsuarioLogado(email);

            if (usuario == null)
            {
                ViewBag.Erro = "Usuário não encontrado.";
                return View();
            }

            if (!_authService.Autenticar(email, senha))
            {
                ViewBag.Erro = "Senha incorreta.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim("Id", usuario.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome ?? usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
           CookieAuthenticationDefaults.AuthenticationScheme,
           new ClaimsPrincipal(identidade));

            // Log de acesso
            var repoLogs = new RepositorioLogsJson();

            var log = new LogAcesso
            {
                UsuarioId = usuario.Id,
                DataHora = DateTime.Now,
                Acao = "Login"
            };

            repoLogs.Registrar(log);


            if (usuario.Perfil == "aluno")
                return RedirectToAction("DashboardAluno");

            return RedirectToAction("DashboardProfessor");
        }

            // ------------------- CADASTRO -------------------
            [HttpGet]
        public IActionResult Cadastro()
        {
            var repoTurmas = new RepositorioTurmasJson();
            var repoUsuarios = new RepositorioUsuariosJson();

            ViewBag.Turmas = repoTurmas.Listar();

            var profCount = repoUsuarios.CarregarUsuarios()
                .Count(u => u.Perfil.Equals("professor", StringComparison.OrdinalIgnoreCase));

            ViewBag.ProfLimitReached = profCount >= 4;

            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(string nome, string email, string senha, string perfil, int? turmaId, int? turmaIdProfessor)
        {
            var repoTurmas = new RepositorioTurmasJson();
            var repoUsuarios = new RepositorioUsuariosJson();

            // sempre recarrega ViewBag em caso de erro
            ViewBag.Turmas = repoTurmas.Listar();

            var profCount = repoUsuarios.CarregarUsuarios()
                .Count(u => u.Perfil.Equals("professor", StringComparison.OrdinalIgnoreCase));

            // ---------------- PROFESSOR ----------------
            if (perfil == "professor")
            {
                if (profCount >= 4)
                {
                    ViewBag.Erro = "Limite de professores atingido.";
                    return View();
                }

                if (!turmaIdProfessor.HasValue)
                {
                    ViewBag.Erro = "Selecione uma turma.";
                    return View();
                }

                var turma = repoTurmas.Listar()
                    .FirstOrDefault(t => t.Id == turmaIdProfessor.Value);

                if (turma == null)
                {
                    ViewBag.Erro = "Turma inválida.";
                    return View();
                }

                if (turma.Professor != 0)
                {
                    ViewBag.Erro = "Essa turma já tem professor.";
                    return View();
                }

                var novoProf = _authService.RegistrarNovoUsuario(email, senha, perfil, turmaIdProfessor.Value, nome);

                turma.Professor = novoProf.Id;
                repoTurmas.SalvarTurmas(repoTurmas.Listar());

                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";
                return RedirectToAction("Login");
            }

            // ---------------- ALUNO ----------------
            if (!turmaId.HasValue)
            {
                ViewBag.Erro = "Selecione uma turma.";
                return View();
            }

            _authService.RegistrarNovoUsuario(email, senha, perfil, turmaId.Value, nome);

            TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";
            return RedirectToAction("Login");
        }

        // ------------------- DASHBOARDS -------------------
        [HttpGet]
        public IActionResult DashboardAluno()
        {
            var emailUsuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var usuario = _authService.ObterUsuarioLogado(emailUsuario);

            if (usuario == null)
                return RedirectToAction("Login");

            var turmasRepo = new RepositorioTurmasJson();
            var turmaAluno = turmasRepo.Listar().FirstOrDefault(t => t.Id == usuario.TurmaId);

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

        // ------------------- LOGOUT -------------------
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
