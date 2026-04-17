using ForFabio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var desenhos = await _db.DesenhosEngenharia.CountAsync();
        var ordensAtivas = await _db.ProducOrders.CountAsync(o => o.Status != "cancelado");
        var emAndamento = await _db.ProducOrders.CountAsync(o => o.Status == "em_andamento");
        var aprovadosCQ = await _db.LaudosQualidadeDetalhados.CountAsync(l => l.Resultado == "APROVADO");

        ViewBag.KpiEng = desenhos;
        ViewBag.KpiPCP = ordensAtivas;
        ViewBag.KpiProd = emAndamento;
        ViewBag.KpiQuali = aprovadosCQ;

        var conjuntos = await _db.DesenhosEngenharia
            .GroupBy(d => d.Conjunto)
            .Select(g => new { Conjunto = g.Key, Total = g.Count() })
            .ToListAsync();

        var laudosAprovados = await _db.LaudosQualidadeDetalhados
            .Where(l => l.Resultado == "APROVADO")
            .ToListAsync();

        // Lógica de progresso por conjunto
        var progressoConjuntos = conjuntos.Select(c => new
        {
            c.Conjunto,
            Total = c.Total,
            Concluidas = laudosAprovados.Count(l => l.Peca.Contains(c.Conjunto)) // simplificação
        });

        ViewBag.ProgressoConjuntos = progressoConjuntos;

        var ultimasOS = await _db.ProducOrders
            .OrderByDescending(o => o.Id)
            .Take(8)
            .ToListAsync();
        ViewBag.UltimasOS = ultimasOS;

        return View();
    }
}