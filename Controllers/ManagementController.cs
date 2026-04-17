using ForFabio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForFab_Metals_System.Controllers
{
   
        [Authorize]
        public class ManagementController : Controller
        {
            private readonly AppDbContext _db;

            public ManagementController(AppDbContext db) => _db = db;

            public async Task<IActionResult> Index()
            {
                var pendentesEng = await _db.DesenhosEngenharia.Where(d => d.StatusAprovacao == "PENDENTE").ToListAsync();
                var pendentesOS = await _db.ProducOrders.Where(o => o.StatusGestor == "PENDENTE").ToListAsync();
                var aprovadosEng = await _db.DesenhosEngenharia.Where(d => d.StatusAprovacao == "APROVADO").ToListAsync();
                var aprovadosOS = await _db.ProducOrders.Where(o => o.StatusGestor == "APROVADO").ToListAsync();
                var apontamentos = await _db.Apontamentos.OrderByDescending(a => a.Timestamp).ToListAsync();

                ViewBag.PendentesEng = pendentesEng;
                ViewBag.PendentesOS = pendentesOS;
                ViewBag.AprovadosEng = aprovadosEng;
                ViewBag.AprovadosOS = aprovadosOS;
                ViewBag.Apontamentos = apontamentos;
                ViewBag.Erros = apontamentos.Where(a => a.Desvio).ToList();

                // KPIs
                ViewBag.TotalOrdensAtivas = await _db.ProducOrders.CountAsync(o => o.Status != "cancelado");
                ViewBag.TotalEstoque = await _db.Almoxarifados.CountAsync();
                ViewBag.TotalDesvios = apontamentos.Count(a => a.Desvio);

                return View();
            }

            [HttpPost]
            public async Task<IActionResult> AprovarDesenho(int id)
            {
                var d = await _db.DesenhosEngenharia.FindAsync(id);
                if (d != null)
                {
                    d.StatusAprovacao = "APROVADO";
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }

            [HttpPost]
            public async Task<IActionResult> RejeitarDesenho(int id, string motivo)
            {
                var d = await _db.DesenhosEngenharia.FindAsync(id);
                if (d != null)
                {
                    d.StatusAprovacao = "REJEITADO";
                    d.ObsGestor = motivo;
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }

            [HttpPost]
            public async Task<IActionResult> AprovarOS(int id)
            {
                var os = await _db.ProducOrders.FindAsync(id);
                if (os != null)
                {
                    os.StatusGestor = "APROVADO";
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
        }
    
}
