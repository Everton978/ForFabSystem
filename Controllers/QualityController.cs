using ForFab_Metals_System.Models;
using ForFabio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class QualityController : Controller
{
    private readonly AppDbContext _db;

    public QualityController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var ordensPendentes = await _db.ProducOrders
            .Where(o => o.Status == "concluido" && !o.LaudoEmitido)
            .ToListAsync();
        var laudos = await _db.LaudosQualidadeDetalhados.OrderByDescending(l => l.Data).ToListAsync();
        ViewBag.Ordens = ordensPendentes;
        return View(laudos);
    }

    [HttpPost]
    public async Task<IActionResult> EmitirLaudo(int osId, string medidaDesenho, string medidaReal, string resultado, string observacao)
    {
        var os = await _db.ProducOrders.FindAsync(osId);
        if (os == null) return NotFound();

        var laudo = new LaudoQualidadeDetalhado
        {
            CodigoOS = os.Id.ToString(),
            Peca = os.Peca,
            MedidaDesenho = medidaDesenho,
            MedidaReal = medidaReal,
            Resultado = resultado,
            Observacao = observacao,
            Inspetor = User.FindFirst(ClaimTypes.Name)?.Value ?? "CQ",
            Data = DateTime.Now
        };
        _db.LaudosQualidadeDetalhados.Add(laudo);

        if (resultado == "APROVADO")
        {
            os.LaudoEmitido = true;
            os.Status = "concluido";
        }
        else if (resultado == "REPROVADO")
        {
            os.Status = "cancelado";
        }
        else if (resultado == "RETRABALHO")
        {
            os.Status = "em_andamento";
            os.AlertaCQ = $"Retrabalho: {observacao}";
            os.Progresso = 0;
            // Resetar tarefas
            var tarefas = await _db.TarefasRoteiro.Where(t => t.OrdemProducaoId == osId).ToListAsync();
            foreach (var t in tarefas) t.Concluida = false;
        }
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}