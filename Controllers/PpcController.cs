using ForFab_Metals_System.Models;
using ForFabio.Data;
using Microsoft.AspNetCore.Mvc;

namespace ForFab_Metals_System.Controllers
{
    public class PpcController : Controller
    {
        private readonly AppDbContext _db;

        public PpcController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var desenhosAprovados = await _db.DesenhosEngenharia
                .Where(d => d.StatusAprovacao == "APROVADO")
                .ToListAsync();
            ViewBag.Desenhos = desenhosAprovados;

            var ordens = await _db.ProducOrders
                .Include(o => o.Tarefas)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
            return View(ordens);
        }

        [HttpPost]
        public async Task<IActionResult> CriarOS(string nomePeca, string linkDesenho, DateTime inicio, DateTime fim, int criticidade, List<string> tarefas)
        {
            var codigo = "#OS-" + new Random().Next(1000, 9999);
            var os = new ProducOrder
            {
                Peca = nomePeca,
                DesenhoURL = linkDesenho,
                DataInicioPlan = inicio,
                DataFimPlan = fim,
                Status = "aguardando",
                StatusGestor = "PENDENTE"
            };
            _db.ProducOrders.Add(os);
            await _db.SaveChangesAsync();

            foreach (var nomeTarefa in tarefas)
            {
                _db.TarefasRoteiro.Add(new TarefaRoteiro
                {
                    OrdemProducaoId = os.Id,
                    Nome = nomeTarefa,
                    Concluida = false
                });
            }
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CalcularTempoTorno(double vc, double d, double f, double l, int passes)
        {
            var rpm = (vc * 1000) / (Math.PI * d);
            var tempo = (l / (f * rpm)) * passes;
            return Json(new { tempo = Math.Round(tempo, 2) });
        }
    }
}
