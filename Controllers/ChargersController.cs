using Microsoft.AspNetCore.Mvc;
using ElectraCharge.Models;
using System.Linq;

namespace ElectraCharge.Controllers
{
    public class ChargersController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor del controlador ChargersController
        public ChargersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar la lista de cargadores
        public IActionResult Index()
        {
            // Obtener la lista de cargadores desde la base de datos
            var chargers = _context.Chargers.ToList();
            return View(chargers);
        }

        // Acción para registrar un nuevo cargador
        [HttpPost]
        public IActionResult Registrar(Charger charger)
        {
            if (ModelState.IsValid)
            {
                // Agregar el cargador a la base de datos
                _context.Chargers.Add(charger);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(charger);
        }

        // Acción para mostrar el formulario de edición de un cargador
        public IActionResult Editar(int id)
        {
            // Buscar el cargador por su id
            var charger = _context.Chargers.Find(id);
            if (charger == null)
            {
                return NotFound();
            }
            return View(charger);
        }

        // Acción para procesar la edición de un cargador
        [HttpPost]
        public IActionResult Editar(int id, Charger charger)
        {
            if (id != charger.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Actualizar el cargador en la base de datos
                _context.Update(charger);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(charger);
        }

        // Acción para mostrar la confirmación de eliminación de un cargador
        public IActionResult Eliminar(int id)
        {
            // Buscar el cargador por su id
            var charger = _context.Chargers.Find(id);
            if (charger == null)
            {
                return NotFound();
            }
            return View(charger);
        }

        // Acción para confirmar la eliminación de un cargador
        [HttpPost, ActionName("Eliminar")]
        public IActionResult ConfirmarEliminar(int id)
        {
            // Buscar el cargador por su id
            var charger = _context.Chargers.Find(id);
            if (charger == null)
            {
                return NotFound();
            }

            // Eliminar el cargador de la base de datos
            _context.Chargers.Remove(charger);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
