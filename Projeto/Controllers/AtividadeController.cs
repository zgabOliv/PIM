using Microsoft.AspNetCore.Mvc;
using Projeto.Data;
using Projeto.Models;
using System.Linq;

public class AtividadeController : Controller
{
    private readonly RepositorioAtividades _repo = new();
    private readonly RepositorioTurmasJson _repoTurmas = new();

    [HttpGet]
    public IActionResult Criar()
    {
        // Carrega todas as turmas
        var turmas = _repoTurmas.Listar();
        ViewBag.Turmas = turmas; // passa turmas completas (não só nomes)
        return View();
    }

    [HttpPost]
    [HttpPost]
    public IActionResult Criar(Atividade atividade)
    {
        if (!ModelState.IsValid)
            return View(atividade);

        var atividades = _repo.Carregar();
        atividade.Id = atividades.Count + 1;
        atividades.Add(atividade);
        _repo.Salvar(atividades);

        // --- Gerar entregas para os alunos reais da turma ---
        var repoEntregas = new RepositorioEntregasJson();
        var entregas = repoEntregas.Carregar();

        var repoUsuarios = new RepositorioUsuariosJson();
        var alunosDaTurma = repoUsuarios.CarregarUsuarios()
            .Where(u => u.Perfil.Equals("aluno", StringComparison.OrdinalIgnoreCase)
                        && u.TurmaId == atividade.TurmaId)
            .ToList();

        foreach (var aluno in alunosDaTurma)
        {
            entregas.Add(new Entrega
            {
                Id = entregas.Count + 1,
                AtividadeId = atividade.Id,
                NomeAluno = aluno.Nome,       // pega o nome real do aluno
                RespostaAluno = "",
                Nota = null,
                FeedbackProfessor = ""
            });
        }

        repoEntregas.Salvar(entregas);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Index()
    {
        var atividades = _repo.Carregar();
        return View(atividades);
    }
}
