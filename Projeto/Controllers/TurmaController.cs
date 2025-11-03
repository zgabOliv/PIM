using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using Projeto.Data;

public class TurmaController : Controller
{
    private readonly RepositorioTurmasJson _repo;
    private readonly RepositorioUsuariosJson _repoUsuarios;

    public TurmaController(RepositorioTurmasJson repo)
    {
        _repo = new RepositorioTurmasJson();
        _repoUsuarios = new RepositorioUsuariosJson();  
    }

    public IActionResult VerAlunos(int id)
    {
        Console.WriteLine($"[DEBUG] Entrou em VerAlunos com id = {id}");

        var todosUsuarios = _repoUsuarios.CarregarUsuarios();
        Console.WriteLine($"[DEBUG] Total de usuários carregados: {todosUsuarios.Count}");

        var alunosDaTurma = todosUsuarios
            .Where(u => u.TurmaId == id && u.Perfil == "aluno")
            .ToList();

        Console.WriteLine($"[DEBUG] Encontrados {alunosDaTurma.Count} alunos na turma {id}");

        ViewBag.NomeTurma = "UNIP";
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
    public IActionResult Index()
    {
        var turmas = _repo.Listar();
        return View(turmas);
    }

    [HttpGet]
    public IActionResult CriarTurma()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CriarTurma(Turma turma)
    {
        if (!ModelState.IsValid)
            return View(turma);

        _repo.Adicionar(turma);
        return RedirectToAction("Index");
    }
}
