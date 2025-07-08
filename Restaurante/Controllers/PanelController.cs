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

        // VER TODAS LAS RESERVAS
        public IActionResult Index()
        {
            var reservas = _db.Reserva.Include(r => r.Mesa).ToList();
            ViewBag.Mesas = _db.Mesa.ToList();
            return View(reservas);
        }

        // CREAR NUEVA RESERVA (GET)
        public IActionResult CrearReserva()
        {
            ViewBag.Mesas = new SelectList(_db.Mesa.Where(m => m.Estado == "disponible"), "Id", "Numero");
            return View();
        }

        // EDITAR RESERVA (GET)
        public IActionResult EditarReserva(int id)
        {
            var reserva = _db.Reserva.Find(id);
            if (reserva == null) return NotFound();

            ViewBag.Mesas = new SelectList(_db.Mesa, "Id", "Numero", reserva.MesaId);
            return View(reserva);
        }

        // EDITAR RESERVA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarReserva(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                // Convertir Fecha a UTC
                reserva.Fecha = DateTime.SpecifyKind(reserva.Fecha, DateTimeKind.Utc);

                _db.Reserva.Update(reserva);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Mesas = new SelectList(_db.Mesa, "Id", "Numero", reserva.MesaId);
            return View(reserva);
        }

        // ELIMINAR RESERVA
        public IActionResult EliminarReserva(int id)
        {
            var reserva = _db.Reserva.Find(id);
            if (reserva == null) return NotFound();

            // Liberar la mesa
            var mesa = _db.Mesa.Find(reserva.MesaId);
            if (mesa != null)
            {
                mesa.Estado = "disponible";
            }

            _db.Reserva.Remove(reserva);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // VER MESAS
        public IActionResult Mesas()
        {
            var mesas = _db.Mesa.ToList();
            return View(mesas);
        }

        // GET: Crear mesa
        public IActionResult CrearMesa()
        {
            return View();
        }

        // POST: Crear mesa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearMesa(Mesa mesa)
        {
            if (ModelState.IsValid)
            {
                _db.Mesa.Add(mesa);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mesa);
        }

        // GET: Editar mesa
        public IActionResult EditarMesa(int id)
        {
            var mesa = _db.Mesa.Find(id);
            if (mesa == null) return NotFound();
            return View(mesa);
        }

        // POST: Editar mesa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarMesa(Mesa mesa)
        {
            if (ModelState.IsValid)
            {
                _db.Mesa.Update(mesa);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mesa);
        }

        // Eliminar mesa
        public IActionResult EliminarMesa(int id)
        {
            var mesa = _db.Mesa.Find(id);
            if (mesa == null) return NotFound();

            _db.Mesa.Remove(mesa);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
