using Microsoft.AspNetCore.Mvc;
using Projeto.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

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
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(string email, string senha, string perfil)
        {
            try
            {
                if (!(perfil.Equals("aluno", StringComparison.OrdinalIgnoreCase) ||
                      perfil.Equals("professor", StringComparison.OrdinalIgnoreCase)))
                {
                    ViewBag.Erro = "Por favor, selecione um tipo de usuário (Aluno ou Professor).";
                    return View();
                }

                _authService.RegistrarNovoUsuario(email, senha, perfil);

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
            return View();
        }

        [HttpGet]
        public IActionResult DashboardProfessor()
        {
            return View();
        }

        // ------------------- LOGOUT -------------------/
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
