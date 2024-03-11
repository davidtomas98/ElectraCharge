using Microsoft.AspNetCore.Mvc;
using ElectraCharge.Models;
using System.Linq;

namespace ElectraCharge.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }
        // Método para mostrar la lista de usuarios con paginación
        public IActionResult Index(int pagina = 1, int cantidadPorPagina = 10)
        {
            // Obtener la lista de usuarios
            var usuarios = _context.Usuarios.ToList();

            // Calcular el total de páginas
            int totalUsuarios = usuarios.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalUsuarios / cantidadPorPagina);

            // Asegurarse de que la página solicitada esté dentro del rango
            pagina = Math.Max(1, Math.Min(pagina, totalPaginas));

            // Obtener los usuarios para la página actual
            var usuariosPagina = usuarios.Skip((pagina - 1) * cantidadPorPagina).Take(cantidadPorPagina).ToList();

            // Pasar los usuarios y la información de paginación a la vista
            ViewBag.Usuarios = usuariosPagina;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(usuarios);
        }

        
        // Método para obtener la lista de usuarios paginada
        public IActionResult Index2(int pagina = 1, int cantidadPorPagina = 10)
        {
            // Calcular el índice de inicio y fin para la paginación
            int indiceInicio = (pagina - 1) * cantidadPorPagina;
            int indiceFin = indiceInicio + cantidadPorPagina;

            // Obtener la lista de usuarios
            var usuarios = _context.Usuarios.ToList();

            // Obtener la lista de usuarios para la página actual
            var usuariosPagina = usuarios.Skip(indiceInicio).Take(cantidadPorPagina);

            // Devolver los usuarios y la información de paginación como JSON
            return Json(new { Usuarios = usuariosPagina, PaginaActual = pagina, TotalPaginas = (int)Math.Ceiling((double)usuarios.Count / cantidadPorPagina) });
        }

        // Acción para filtrar usuarios según el criterio de búsqueda
        [HttpGet]
        public IActionResult FiltrarUsuarios(string filtro, string valor)
        {
            IQueryable<Usuario> usuarios = _context.Usuarios;

            // Verificar si se proporciona un valor de búsqueda
            if (!string.IsNullOrEmpty(valor))
            {
                // Aplicar el filtro según el criterio seleccionado
                if (filtro == "nombre")
                {
                    usuarios = usuarios.Where(u => u.Nombre.StartsWith(valor));
                }
                else if (filtro == "apellido1")
                {
                    usuarios = usuarios.Where(u => u.Apellido1.StartsWith(valor));
                }
                else if (filtro == "correoElectronico")
                {
                    usuarios = usuarios.Where(u => u.CorreoElectronico.StartsWith(valor));
                }
            }
            else
            {
                // Si el valor de búsqueda es nulo, mostrar todos los usuarios
                return Index(); // Llama al método Index para mostrar todos los usuarios
            }

            // Obtener los usuarios paginados
            var usuariosPaginados = usuarios.ToList();

            // Devolver los datos de usuarios y la información de paginación como JSON
            return Json(new { usuarios = usuariosPaginados });
        }


        // Acción para obtener los detalles de un usuario
        [HttpGet]
        public IActionResult ObtenerUsuario(int id)
        {
            // Buscar el usuario en la base de datos
            var usuario = _context.Usuarios.Find(id);

            if (usuario != null)
            {
                // Devolver los detalles del usuario en formato JSON
                return Json(usuario);
            }
            else
            {
                // Devolver un mensaje de error si el usuario no se encuentra
                return Json(new { error = "Usuario no encontrado" });
            }
        }



        // Método para registrar un nuevo usuario
        [HttpPost]
        public IActionResult Registrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Obtener el último ID de usuario en la base de datos
                var ultimoId = _context.Usuarios.OrderByDescending(u => u.Id).Select(u => u.Id).FirstOrDefault();
                
                // Asignar el nuevo ID incrementado en uno al usuario
                usuario.Id = ultimoId + 1;

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

            // Eliminar también las asignaciones relacionadas
            var asignaciones = _context.Asignaciones.Where(a => a.IdUsuario == id);
            _context.Asignaciones.RemoveRange(asignaciones);

            // Eliminar el usuario de la base de datos
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
