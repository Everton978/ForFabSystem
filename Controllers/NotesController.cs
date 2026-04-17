using ForFabio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ForFab_Metals_System.Controllers
{
 
        [Authorize]
        public class NotesController : Controller
        {
            private readonly AppDbContext _db;

            public NotesController(AppDbContext db) => _db = db;

            public IActionResult Index() => View();

            [HttpPost]
            public async Task<IActionResult> Registrar(Apontamento model)
            {
                model.RA = User.FindFirst("RA")?.Value!;
                model.NomeOperador = User.FindFirst(ClaimTypes.Name)?.Value!;
                model.Timestamp = DateTime.Now;
                _db.Apontamentos.Add(model);
                await _db.SaveChangesAsync();
                TempData["Sucesso"] = "Apontamento registrado!";
                return RedirectToAction("Index");
            }
        }
    
}
