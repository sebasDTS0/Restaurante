using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Restaurante.Controllers
{
    public class CuentaController : Controller
    {
        private const string ADMIN_CORREO = "admin@tradiciones.com";
        private const string ADMIN_PASSWORD = "Admin123";

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string contrasena)
        {
            if (correo == ADMIN_CORREO && contrasena == ADMIN_PASSWORD)
            {
                HttpContext.Session.SetString("Usuario", correo);
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
