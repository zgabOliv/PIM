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
    [HttpGet]
    [HttpGet]
    public IActionResult Index()
    {
        var userId = int.Parse(User.FindFirst("Id").Value);

        var repoUsuarios = new RepositorioUsuariosJson();
        var usuario = repoUsuarios.CarregarUsuarios()
            .FirstOrDefault(u => u.Id == userId);

        if (usuario == null)
            return Unauthorized();

        var repoTurmas = new RepositorioTurmasJson();

        // Se for professor: pegar todas turmas dele
        if (usuario.Perfil.ToLower() == "professor")
        {
            var turmasDoProfessor = repoTurmas.Listar()
                .Where(t => t.Professor == usuario.Id)
                .Select(t => t.Id)
                .ToList();

            var atividadesProf = _repo.Carregar()
                .Where(a => turmasDoProfessor.Contains(a.TurmaId))
                .ToList();

            return View(atividadesProf);
        }

        // Se for aluno: filtra pela turma única
        if (usuario.Perfil.ToLower() == "aluno")
        {
            var atividadesAluno = _repo.Carregar()
                .Where(a => a.TurmaId == usuario.TurmaId)
                .ToList();

            return View(atividadesAluno);
        }

        return Unauthorized();
    }
}