using ForFab_Metals_System.Models;
using ForFabio.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ForFab_Metals_System.Controllers
{
    public class EngineeringController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public EngineeringController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var desenhos = await _db.DesenhosEngenharia
                .Include(d => d.Arquivo)
                .OrderByDescending(d => d.DataCriacao)
                .ToListAsync();
            return View(desenhos);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(DesenhoEngenharia model, IFormFile arquivo)
        {
            model.Autor = User.FindFirst(ClaimTypes.Name)?.Value ?? "Engenheiro";
            model.StatusAprovacao = "PENDENTE";
            model.DataCriacao = DateTime.Now;

            if (arquivo != null && arquivo.Length > 0)
            {
                using var ms = new MemoryStream();
                await arquivo.CopyToAsync(ms);
                var fileEntity = new FileEntity
                {
                    Nome = arquivo.FileName,
                    Conteudo = ms.ToArray()
                };
                _db.Files.Add(fileEntity);
                await _db.SaveChangesAsync();
                model.FileId = fileEntity.Id;
            }

            _db.DesenhosEngenharia.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, DesenhoEngenharia model, IFormFile arquivo)
        {
            var desenho = await _db.DesenhosEngenharia.FindAsync(id);
            if (desenho == null) return NotFound();

            desenho.Conjunto = model.Conjunto;
            desenho.Rev = model.Rev;
            desenho.Tipo = model.Tipo;
            desenho.MaterialSugerido = model.MaterialSugerido;
            desenho.Link = model.Link;
            desenho.StatusAprovacao = "PENDENTE";

            if (arquivo != null)
            {
                using var ms = new MemoryStream();
                await arquivo.CopyToAsync(ms);
                var fileEntity = new FileEntity { Nome = arquivo.FileName, Conteudo = ms.ToArray() };
                _db.Files.Add(fileEntity);
                await _db.SaveChangesAsync();
                desenho.FileId = fileEntity.Id;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
