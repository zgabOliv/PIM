using Microsoft.AspNetCore.Mvc;
using Projeto.Data;
using Projeto.Models;
using Projeto.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

public class AlunoController : Controller
{
    private readonly RepositorioUsuariosJson _repoUsuarios = new();
    private readonly RepositorioTurmasJson _repoTurmas = new();
    private readonly RepositorioAtividades _repoAtividades = new();
    private readonly RepositorioEntregasJson _repoEntregas = new();

    [HttpGet]
    public IActionResult Dashboard()
    {
        var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (email == null)
            return RedirectToAction("Login", "HelloWorld");

        var usuario = _repoUsuarios.CarregarUsuarios()
                                   .FirstOrDefault(u => u.Email == email);
        if (usuario == null)
            return RedirectToAction("Login", "HelloWorld");

        var turmaAluno = _repoTurmas.Listar()
                                    .FirstOrDefault(t => t.Id == usuario.TurmaId);

        ViewBag.Turma = turmaAluno;
        return View(usuario);
    }

    [HttpGet]
    [HttpGet]
    public IActionResult Atividades()
    {
        // Pega o email do usuário logado
        var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(email))
            return RedirectToAction("Login", "HelloWorld");

        // Busca o usuário pelo email
        var usuario = _repoUsuarios.CarregarUsuarios()
            .FirstOrDefault(u => u.Email == email);

        if (usuario == null)
            return RedirectToAction("Login", "HelloWorld");

        // Filtra atividades da turma do usuário
        var atividades = _repoAtividades.Carregar()
            .Where(a => a.TurmaId == usuario.TurmaId)
            .ToList();

        // Passa a lista de atividades para a view
        return View(atividades);
    }

    public IActionResult Turmas(int? id)
    {
        // Pega o email do usuário logado
        var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(email))
            return RedirectToAction("Login", "HelloWorld");

        // Busca o usuário
        var usuario = _repoUsuarios.CarregarUsuarios()
            .FirstOrDefault(u => u.Email == email);

        if (usuario == null)
            return RedirectToAction("Login", "HelloWorld");

        // Carrega todas as turmas
        var todasTurmas = _repoTurmas.CarregarTurmas();

        // Seleciona apenas a turma do aluno
        Projeto.Models.Turma turmaSelecionada = null;
        if (id.HasValue)
        {
            turmaSelecionada = todasTurmas.FirstOrDefault(t => t.Id == id && t.Id == usuario.TurmaId);

            if (turmaSelecionada == null)
            {
                // Se tentou acessar outra turma que não é dele
                return RedirectToAction("Dashboard", "Aluno");
            }
        }

        ViewBag.TurmaSelecionada = turmaSelecionada;

        return View(todasTurmas);
    }



    [HttpGet]
    public IActionResult EnviarAtividade(int atividadeId)
    {
        var atividade = _repoAtividades.Carregar().FirstOrDefault(a => a.Id == atividadeId);
        if (atividade == null) return NotFound();

        var vm = new EntregaAlunoViewModel
        {
            AtividadeId = atividade.Id,
            TituloAtividade = atividade.Titulo
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> EnviarAtividade(EntregaAlunoViewModel vm)
    {
        var emailAluno = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var aluno = _repoUsuarios.CarregarUsuarios().FirstOrDefault(u => u.Email == emailAluno);
        if (aluno == null) return Unauthorized();

        string? caminhoArquivo = null;

        if (vm.ArquivoAluno != null && vm.ArquivoAluno.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Path.GetFileName(vm.ArquivoAluno.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await vm.ArquivoAluno.CopyToAsync(stream);
            }

            caminhoArquivo = "/uploads/" + fileName;
        }

        var entregas = _repoEntregas.Carregar();
        var novoId = entregas.Any() ? entregas.Max(e => e.Id) + 1 : 1;
        var atividade = _repoAtividades.Carregar().FirstOrDefault(a => a.Id == vm.AtividadeId);

        entregas.Add(new Entrega
        {
            Id = novoId,
            AtividadeId = vm.AtividadeId,
            NomeAluno = aluno.Email,
            RespostaAluno = vm.RespostaAluno,
            Nota = null,
            FeedbackProfessor = string.Empty,
            TituloAtividade = atividade?.Titulo ?? string.Empty,
            TurmaId = aluno.TurmaId,
            CaminhoArquivo = caminhoArquivo
        });

        _repoEntregas.Salvar(entregas);

        return RedirectToAction("Atividades");
    }
    [HttpGet]



    [HttpGet]
    public IActionResult Notas()
    {
        var emailAluno = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (emailAluno == null)
            return RedirectToAction("Login", "HelloWorld");

        var aluno = _repoUsuarios.CarregarUsuarios()
                                 .FirstOrDefault(u => u.Email == emailAluno);
        if (aluno == null)
            return RedirectToAction("Login", "HelloWorld");

        var entregasAluno = _repoEntregas.Carregar()
            .Where(e => e.NomeAluno == aluno.Email)

            .Select(e => new EntregaViewNota
            {
                AtividadeId = e.AtividadeId,
                RespostaAluno = e.RespostaAluno,
                Nota = e.Nota,
                FeedbackProfessor = e.FeedbackProfessor
            })
            .ToList();

        return View(entregasAluno);
    }
}
