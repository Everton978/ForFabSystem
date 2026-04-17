using ForFab_Metals_System.Models;
using ForFabio.Data;
using ForFabio.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ForFabio.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string ra, string senha)
    {
        var hash = HashSenha(senha);
        var user = await _db.Users.FirstOrDefaultAsync(u => u.RA == ra && u.Senha == hash);
        if (user == null)
        {
            ModelState.AddModelError("", "RA ou senha inválidos");
            return View();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Nome),
            new("RA", user.RA)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Cadastro() => View();

    [HttpPost]
    public async Task<IActionResult> Cadastro(User model, string senha)
    {
        if (await _db.Users.AnyAsync(u => u.RA == model.RA))
        {
            ModelState.AddModelError("RA", "RA já cadastrado");
            return View(model);
        }
        model.Senha = HashSenha(senha);
        model.DataCadastro = DateTime.Now.ToString("yyyy-MM-dd");
        _db.Users.Add(model);
        await _db.SaveChangesAsync();
        return RedirectToAction("Login");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }

    private string HashSenha(string senha)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(senha);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}