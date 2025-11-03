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

    public IActionResult VerAlunos(int idTurma)
    {
        // pega a turma
        var turma = _repo.Listar().FirstOrDefault(t => t.Id == idTurma);
        if (turma == null)
            return NotFound();

        // pega todos os usuários
        var usuarios = _repoUsuarios.CarregarUsuarios();

        // filtra os alunos da turma
        var alunos = usuarios.Where(u => u.TurmaId == idTurma && u.Perfil == "Aluno").ToList();

        ViewBag.NomeTurma = turma.Nome;
        return View(alunos);
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
