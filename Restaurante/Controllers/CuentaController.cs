using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Restaurante.Controllers
{
    public class CuentaController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string usuario, string contrasena)
        {
            if (usuario == "admin@tradiciones.com" && contrasena == "Admin123")
            {
                HttpContext.Session.SetString("Usuario", usuario);
                return RedirectToAction("Index", "Panel");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
