using Microsoft.AspNetCore.Mvc;
using Projeto.Data;
using Projeto.Models;
using Projeto.ViewModels;
using System.Linq;

public class EntregaController : Controller
{
    private readonly RepositorioEntregasJson _repoEntregas = new();
    private readonly RepositorioAtividades _repoAtividades = new RepositorioAtividades();

    public EntregaController()
    {
        // Garantir que o JSON exista
        if (!System.IO.File.Exists("Data/entregas.json"))
        {
            _repoEntregas.Salvar(new List<Entrega>()); // cria o JSON vazio
        }
    }

    [HttpGet]
    public IActionResult Index()
    {
        var entregas = _repoEntregas.Carregar();

        var vm = entregas.Select(e => new EntregaViewModel
        {
            Id = e.Id,
            NomeAluno = e.NomeAluno,
            Nota = e.Nota
        }).ToList();

        return View(vm);
    }

    // Método GET para o professor ver todas as entregas
    [HttpGet]
    public IActionResult Corrigir()
    {
        var entregas = _repoEntregas.Carregar();
        var atividades = _repoAtividades.Carregar();

        // Converte para ViewModel com título da atividade
        var vmList = entregas.Select(e => new EntregaCorrigirViewModel
        {
            Id = e.Id,
            AtividadeId = e.AtividadeId,
            NomeAluno = e.NomeAluno,
            RespostaAluno = e.RespostaAluno,
            Nota = e.Nota,
            FeedbackProfessor = e.FeedbackProfessor
        }).ToList();

        return View(vmList); // Passa a lista para a view Corrigir
    }

 
    [HttpPost]
    public IActionResult Corrigir(EntregaCorrigirViewModel vm)
    {
        var entregas = _repoEntregas.Carregar();
        var entrega = entregas.FirstOrDefault(e => e.Id == vm.Id); // <-- aqui pega o id correto

        if (entrega == null)
            return NotFound("Entrega não encontrada!");

        entrega.Nota = vm.Nota;
        entrega.FeedbackProfessor = vm.FeedbackProfessor;

        _repoEntregas.Salvar(entregas);

        return RedirectToAction("Corrigir");
    }
}