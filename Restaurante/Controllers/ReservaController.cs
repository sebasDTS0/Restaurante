using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Datos;
using Restaurante.Models;

namespace Restaurante.Controllers
{
    public class ReservaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReservaController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Mostrar formulario de reserva
        public IActionResult Index()
        {
            var mesasDisponibles = _db.Mesa
                .Where(m => m.Estado == "disponible")
                .ToList();

            ViewBag.Mesas = new SelectList(mesasDisponibles, "Id", "Numero");
            ViewBag.MesasDisponibles = mesasDisponibles;

            return View(new Reserva());
        }

        // Guardar la reserva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                // Forzar fecha como UTC para evitar conflicto
                reserva.Fecha = DateTime.SpecifyKind(reserva.Fecha, DateTimeKind.Utc);
                reserva.FechaCreacion = DateTime.UtcNow;

                _db.Reserva.Add(reserva);

                var mesa = _db.Mesa.FirstOrDefault(m => m.Id == reserva.MesaId);
                if (mesa != null)
                {
                    mesa.Estado = "ocupada";
                    _db.Mesa.Update(mesa);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Mesas = new SelectList(_db.Mesa.Where(m => m.Estado == "disponible"), "Id", "Numero", reserva.MesaId);
            ViewBag.MesasDisponibles = _db.Mesa.Where(m => m.Estado == "disponible").ToList();

            return View("Index", reserva);
        }
    }
}
