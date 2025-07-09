using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Datos;
using Restaurante.Models;

namespace Restaurante.Controllers
{
    public class PanelController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PanelController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Verifica si el usuario está autenticado (solo permite admin)
        private bool UsuarioAutenticado()
        {
            return HttpContext.Session.GetString("Usuario") == "admin@tradiciones.com";
        }

        public IActionResult Index()
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            var reservas = _db.Reserva.Include(r => r.Mesa).ToList();
            ViewBag.Mesas = _db.Mesa.ToList();
            return View(reservas);
        }

        public IActionResult CrearReserva()
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            ViewBag.Mesas = new SelectList(_db.Mesa.Where(m => m.Estado == "disponible"), "Id", "Numero");
            var reserva = new Reserva { Fecha = DateTime.Now };
            return View();
        }

        public IActionResult EditarReserva(int id)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            var reserva = _db.Reserva.Find(id);
            if (reserva == null) return NotFound();

            ViewBag.Mesas = new SelectList(_db.Mesa, "Id", "Numero", reserva.MesaId);
            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarReserva(Reserva reserva)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            if (ModelState.IsValid)
            {
                reserva.Fecha = DateTime.SpecifyKind(reserva.Fecha, DateTimeKind.Utc);
                _db.Reserva.Update(reserva);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Mesas = new SelectList(_db.Mesa, "Id", "Numero", reserva.MesaId);
            return View(reserva);
        }

        public IActionResult EliminarReserva(int id)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            var reserva = _db.Reserva.Find(id);
            if (reserva == null) return NotFound();

            var mesa = _db.Mesa.Find(reserva.MesaId);
            if (mesa != null)
            {
                mesa.Estado = "disponible";
            }

            _db.Reserva.Remove(reserva);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Mesas()
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            var mesas = _db.Mesa.ToList();
            return View(mesas);
        }

        public IActionResult CrearMesa()
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearMesa(Mesa mesa)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            if (ModelState.IsValid)
            {
                _db.Mesa.Add(mesa);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mesa);
        }

        public IActionResult EditarMesa(int id)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            var mesa = _db.Mesa.Find(id);
            if (mesa == null) return NotFound();
            return View(mesa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarMesa(Mesa mesa)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            if (ModelState.IsValid)
            {
                _db.Mesa.Update(mesa);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mesa);
        }

        public IActionResult EliminarMesa(int id)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Cuenta");

            var mesa = _db.Mesa.Find(id);
            if (mesa == null) return NotFound();

            _db.Mesa.Remove(mesa);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
