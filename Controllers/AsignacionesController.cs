using Microsoft.AspNetCore.Mvc;
using ElectraCharge.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ElectraCharge.Controllers
{
    public class AsignacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsignacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para mostrar la vista de asignación de cargador a usuario
        public IActionResult Index()
        {
            var asignaciones = _context.Asignaciones
                .Include(a => a.Usuarios) // Incluye la lista de usuarios relacionados
                .Include(a => a.Cargadores) // Incluye la lista de cargadores relacionados
                .ToList();

            return View(asignaciones);
        }

        // Método para procesar la asignación de cargador a usuario
        [HttpPost]
        public IActionResult Asignar(Asignar asignacion)
        {
            if (ModelState.IsValid)
            {
                _context.Asignaciones.Add(asignacion);
                _context.SaveChanges();

                return RedirectToAction("Index", "Usuarios");
            }

            // Si hay un error en el modelo, volvemos a mostrar la vista con el modelo y los errores
            asignacion.Usuarios = _context.Usuarios.ToList();
            asignacion.Cargadores = _context.Cargadores.Where(c => c.Estado == "disponible").ToList();

            return View(asignacion);
        }
    }
}
