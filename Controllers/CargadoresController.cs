using Microsoft.AspNetCore.Mvc;
using ElectraCharge.Models;
using System.Linq;

namespace ElectraCharge.Controllers
{
    public class CargadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor del controlador CargadoresController
        public CargadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar la lista de cargadores
        public IActionResult Index()
        {
            // Obtener la lista de cargadores desde la base de datos
            var cargadores = _context.Cargadores.ToList();
            return View(cargadores);
        }

        // Acción para registrar un nuevo cargador
        [HttpPost]
        public IActionResult Registrar(Cargador cargador)
        {
            if (ModelState.IsValid)
            {
                // Agregar el cargador a la base de datos
                _context.Cargadores.Add(cargador);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cargador);
        }

        // Acción para mostrar el formulario de edición de un cargador
        public IActionResult Editar(int id)
        {
            // Buscar el cargador por su id
            var cargador = _context.Cargadores.Find(id);
            if (cargador == null)
            {
                return NotFound();
            }
            return View(cargador);
        }

        // Acción para procesar la edición de un cargador
        [HttpPost]
        public IActionResult Editar(int id, Cargador cargador)
        {
            if (id != cargador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Actualizar el cargador en la base de datos
                _context.Update(cargador);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cargador);
        }

        // Acción para mostrar la confirmación de eliminación de un cargador
        public IActionResult Eliminar(int id)
        {
            // Buscar el cargador por su id
            var cargador = _context.Cargadores.Find(id);
            if (cargador == null)
            {
                return NotFound();
            }
            return View(cargador);
        }

        // Acción para confirmar la eliminación de un cargador
        [HttpPost, ActionName("Eliminar")]
        public IActionResult ConfirmarEliminar(int id)
        {
            // Buscar el cargador por su id
            var cargador = _context.Cargadores.Find(id);
            if (cargador == null)
            {
                return NotFound();
            }

            // Eliminar el cargador de la base de datos
            _context.Cargadores.Remove(cargador);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
