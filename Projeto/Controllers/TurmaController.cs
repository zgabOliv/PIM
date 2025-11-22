using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projeto.Data;
using Projeto.Models;
using System.Security.Claims;


public class TurmaController : Controller
{
    private readonly RepositorioTurmasJson _repo;
    private readonly RepositorioUsuariosJson _repoUsuarios;
    private readonly ILogger<TurmaController> _logger;
    public TurmaController(RepositorioTurmasJson repo, ILogger<TurmaController> logger)
    {
        _repo = new RepositorioTurmasJson();
        _repoUsuarios = new RepositorioUsuariosJson();
        _logger = logger;
    }

    public IActionResult VerAlunos(int id)
    {
        // Pega email do professor logado
        var emailProfessor = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(emailProfessor))
            return RedirectToAction("Login", "HelloWorld");

        // Busca professor pelo email
        var professor = _repoUsuarios.CarregarUsuarios()
                             .FirstOrDefault(u => u.Email == emailProfessor && u.Perfil == "professor");
        if (professor == null)
            return Forbid();

        // Busca a turma
        var turma = _repo.Listar().FirstOrDefault(t => t.Id == id);
        if (turma == null)
            return NotFound();

        // Verifica se a turma pertence a esse professor
        if (turma.Professor != professor.Id)
        {
            // Se não for dele, pode apenas mostrar mensagem vazia
            ViewBag.NomeTurma = turma.Nome;
            ViewBag.Mensagem = "Essa turma não pertence a você.";
            return View(new List<Usuario>());
        }

        // Busca alunos da turma
        var alunosDaTurma = _repoUsuarios.CarregarUsuarios()
                                .Where(u => u.TurmaId == id && u.Perfil == "aluno")
                                .ToList();

        ViewBag.NomeTurma = turma.Nome;

        // Se não tiver alunos, a View vai mostrar mensagem padrão
        return View(alunosDaTurma);
    }



    public IActionResult Detalhes(int id)
    {
        var turma = _repo.Listar().FirstOrDefault(t => t.Id == id);
        if (turma == null)
            return NotFound();

        var alunos = _repoUsuarios.CarregarUsuarios()
            .Where(u => u.Perfil.Equals("aluno", StringComparison.OrdinalIgnoreCase) && u.TurmaId == id)
            .ToList();

        ViewBag.Turma = turma;
        ViewBag.Alunos = alunos;

        return View();
    }

    [HttpGet]
    [HttpGet]
    [HttpGet]
    [HttpGet]
    [HttpGet]
    public IActionResult Index()
    {
        var emailProfessor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine("Email do professor logado: " + emailProfessor);

        var professor = _repoUsuarios.CarregarUsuarios()
                             .FirstOrDefault(u => u.Email == emailProfessor && u.Perfil == "professor");

        if (professor == null)
        {
            Console.WriteLine("Professor não encontrado!");
            return Forbid();
        }

        Console.WriteLine($"Professor encontrado: {professor.Nome}, Id: {professor.Id}");

        var turmasDoProfessor = _repo.Listar()
                                     .Where(t => t.Professor == professor.Id)
                                     .ToList();

        Console.WriteLine($"Quantidade de turmas do professor: {turmasDoProfessor.Count}");
        foreach (var t in turmasDoProfessor)
        {
            Console.WriteLine($"Turma: {t.Nome}, Id: {t.Id}, ProfessorId: {t.Professor}");
        }

        // Pra ver também no ViewBag (útil se quiser mostrar na View)
        ViewBag.EmailProfessor = emailProfessor;
        ViewBag.ProfessorId = professor.Id;

        return View(turmasDoProfessor);
    }





    [HttpGet]
    public IActionResult CriarTurma()
    {
        return View();
    }

    [HttpPost]

    public IActionResult CriarTurma(Turma turma)
    {
        var emailProfessor = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var professor = _repoUsuarios.CarregarUsuarios()
                             .FirstOrDefault(u => u.Email == emailProfessor && u.Perfil == "professor");

        if (professor == null)
            return Forbid();

        turma.Professor = professor.Id; // vincula a turma ao professor logado
        _repo.Adicionar(turma);

        return RedirectToAction("Index");
    }
}
