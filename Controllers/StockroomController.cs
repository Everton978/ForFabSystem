using ForFabio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForFab_Metals_System.Controllers
{

        [Authorize]
        public class StockroomController : Controller
        {
            private readonly AppDbContext _db;

            public StockroomController(AppDbContext db) => _db = db;

            public async Task<IActionResult> Index()
            {
                var itens = await _db.Almoxarifados.OrderByDescending(i => i.DataEntrada).ToListAsync();
                var desenhos = await _db.DesenhosEngenharia.Where(d => d.StatusAprovacao == "APROVADO").ToListAsync();
                ViewBag.Desenhos = desenhos;
                return View(itens);
            }

            [HttpPost]
            public async Task<IActionResult> Adicionar(Almoxarifado item)
            {
                item.DataEntrada = DateTime.Now;
                _db.Almoxarifados.Add(item);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            [HttpPost]
            public async Task<IActionResult> AtualizarStatus(int id, string status)
            {
                var item = await _db.Almoxarifados.FindAsync(id);
                if (item != null)
                {
                    item.Status = status;
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
        }
    
}
