using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using Projeto.Data;

public class TurmaController : Controller
{
    private readonly RepositorioTurmasJson _repo;

    public TurmaController(RepositorioTurmasJson repo)
    {
        _repo = repo;
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
