using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using System.Collections.Generic;

public class AtividadeController : Controller
{
    private static List<Atividade> atividades = new List<Atividade>();

    [HttpGet]
    public IActionResult Criar()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Criar(Atividade atividade)
    {
        atividade.Id = atividades.Count + 1;
        atividades.Add(atividade);
        return RedirectToAction("Index"); // depois cria a view Index para listar
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(atividades);
    }
}
