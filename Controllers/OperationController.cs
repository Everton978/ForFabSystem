using Microsoft.AspNetCore.Mvc;

namespace ForFab_Metals_System.Controllers
{
    public class OperationController : Controller
    {
        private readonly AppDbContext _db;
        public OperationController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index(string? busca, string? filtroStatus)
        {
            var query = _db.ProducOrders
                .Include(o => o.Tarefas)
                .Where(o => o.Status != "cancelado"); // adapte conforme necessidade

            if (!string.IsNullOrEmpty(busca))
                query = query.Where(o => o.Peca.Contains(busca) || o.Id.ToString().Contains(busca));
            if (!string.IsNullOrEmpty(filtroStatus) && filtroStatus != "todas")
                query = query.Where(o => o.Status == filtroStatus);

            var ordens = await query.OrderByDescending(o => o.Id).ToListAsync();
            return View(ordens);
        }

        [HttpPost]
        public async Task<IActionResult> Assumir(int id)
        {
            var os = await _db.ProducOrders.FindAsync(id);
            if (os == null) return NotFound();

            var ra = User.FindFirst("RA")?.Value;
            os.ResponsavelRA = ra;
            os.Status = "em_andamento";
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MudarStatus(int id, string novoStatus)
        {
            var os = await _db.ProducOrders.Include(o => o.Tarefas).FirstOrDefaultAsync(o => o.Id == id);
            if (os == null) return NotFound();

            if (novoStatus == "concluido" && os.Tarefas.Any(t => !t.Concluida))
            {
                TempData["Erro"] = "Complete todas as tarefas antes de finalizar.";
                return RedirectToAction("Index");
            }

            os.Status = novoStatus;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarProgresso(int id, Dictionary<int, bool> tarefasConcluidas)
        {
            var os = await _db.ProducOrders.Include(o => o.Tarefas).FirstOrDefaultAsync(o => o.Id == id);
            if (os == null) return NotFound();

            foreach (var tarefa in os.Tarefas)
                if (tarefasConcluidas.TryGetValue(tarefa.Id, out var concluida))
                    tarefa.Concluida = concluida;

            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
