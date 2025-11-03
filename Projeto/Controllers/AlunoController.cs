using Microsoft.AspNetCore.Mvc;
using Projeto.Data;
using Projeto.Models;
using Projeto.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
        var email = User.FindFirst(ClaimTypes.Name)?.Value;
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
    public IActionResult Atividades()
    {
        var email = User.FindFirst(ClaimTypes.Name)?.Value;
        var usuario = _repoUsuarios.CarregarUsuarios()
                                   .FirstOrDefault(u => u.Email == email);
        if (usuario == null)
            return RedirectToAction("Login", "HelloWorld");

        var atividades = _repoAtividades.Carregar()
                                        .Where(a => a.TurmaId == usuario.TurmaId)
                                        .ToList();

        return View(atividades);
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

    // Enviar resposta da atividade (POST)
    [HttpPost]
    public async Task<IActionResult> EnviarAtividade(EntregaAlunoViewModel vm)
    {
        var emailAluno = User.FindFirst(ClaimTypes.Name)?.Value;
        var aluno = _repoUsuarios.CarregarUsuarios().FirstOrDefault(u => u.Email == emailAluno);
        if (aluno == null) return Unauthorized();

        string? caminhoArquivo = null;

        // Salvar arquivo, se houver
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

            caminhoArquivo = "/uploads/" + fileName; // caminho virtual
        }

        var entregas = _repoEntregas.Carregar();
        var novoId = entregas.Any() ? entregas.Max(e => e.Id) + 1 : 1;
        var atividade = _repoAtividades.Carregar().FirstOrDefault(a => a.Id == vm.AtividadeId);

        entregas.Add(new Entrega
        {
            Id = novoId,
            AtividadeId = vm.AtividadeId,
            NomeAluno = string.IsNullOrWhiteSpace(aluno.Nome) ? aluno.Email : aluno.Nome,
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


    // notas do aluno 
    [HttpGet]
    // GET: Aluno/Notas
    [HttpGet]
    public IActionResult Notas()
    {
        var emailAluno = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        if (emailAluno == null)
            return RedirectToAction("Login", "HelloWorld");

        var aluno = _repoUsuarios.CarregarUsuarios()
                                 .FirstOrDefault(u => u.Email == emailAluno);
        if (aluno == null)
            return RedirectToAction("Login", "HelloWorld");

        // Pega todas as entregas do aluno
        var entregasAluno = _repoEntregas.Carregar()
       .Where(e => e.NomeAluno == aluno.Email) // filtra pelo email do aluno
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
        public async Task<IActionResult> Upload(EntregaAlunoViewModel vm)
        {
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

                // salva caminho relativo com barra inicial
                vm.CaminhoArquivo = "/uploads/" + fileName;
                ViewBag.Mensagem = "Arquivo enviado com sucesso!";
                ViewBag.CaminhoArquivo = vm.CaminhoArquivo; // vai preencher o campo oculto no form
            }
            else
            {
                ViewBag.Mensagem = "Nenhum arquivo selecionado.";
            }

            return View("EnviarAtividade", vm); // volta pra mesma view
        }
    }