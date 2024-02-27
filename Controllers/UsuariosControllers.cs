using Microsoft.AspNetCore.Mvc;
using ElectraCharge.Models;
using System.Linq;

namespace ElectraCharge.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor que recibe el contexto de la base de datos
        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para mostrar la lista de usuarios
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        // Método para registrar un nuevo usuario
        [HttpPost]
        public IActionResult Registrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // Método para mostrar la vista de edición de un usuario
        public IActionResult Editar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // Método para procesar la edición de un usuario
        [HttpPost]
        public IActionResult Editar(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(usuario);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // Método para mostrar la vista de eliminación de un usuario
        public IActionResult Eliminar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // Método para confirmar la eliminación de un usuario
        [HttpPost, ActionName("Eliminar")]
        public IActionResult ConfirmarEliminar(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
