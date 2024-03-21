using Microsoft.AspNetCore.Mvc;
using ElectraCharge.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;


namespace ElectraCharge.Controllers
{
    public class CargadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor del controlador CargadoresController
        public CargadoresController(ApplicationDbContext context )
        {
            _context = context;
        }

        // Acción para mostrar la lista de cargadores con paginación
        [Authorize]
        public IActionResult Index(int pagina = 1, int cantidadPorPagina = 12)
        {
            // Obtener la lista de cargadores
            var cargadores = _context.Cargadores.ToList();

            // Calcular el total de páginas
            int totalCargadores = cargadores.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalCargadores / cantidadPorPagina);

            // Asegurarse de que la página solicitada esté dentro del rango
            pagina = Math.Max(1, Math.Min(pagina, totalPaginas));

            // Obtener los cargadores para la página actual
            var cargadoresPagina = cargadores.Skip((pagina - 1) * cantidadPorPagina).Take(cantidadPorPagina).ToList();

            // Pasar los cargadores y la información de paginación a la vista
            ViewBag.Cargadores = cargadoresPagina;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(cargadoresPagina);
        }


        // Método para obtener la lista de cargadores paginada
        public IActionResult Index2(int pagina = 1, int cantidadPorPagina = 12)
        {
            // Calcular el índice de inicio y fin para la paginación
            int indiceInicio = (pagina - 1) * cantidadPorPagina;

            // Obtener la lista de cargadores
            var cargadores = _context.Cargadores.ToList();

            // Obtener la lista de cargadores para la página actual
            var cargadoresPagina = cargadores.Skip(indiceInicio).Take(cantidadPorPagina).ToList();

            // Devolver los cargadores y la información de paginación como JSON
            return Json(new { Cargadores = cargadoresPagina, PaginaActual = pagina, TotalPaginas = (int)Math.Ceiling((double)cargadores.Count / cantidadPorPagina) });
        }

        // Acción para filtrar cargadores según el criterio de búsqueda
        [HttpGet]
        public IActionResult FiltrarCargadores(string filtro, string valor)
        {
            IQueryable<Cargador> cargadores = _context.Cargadores;

            // Verificar si se proporciona un valor de búsqueda
            if (!string.IsNullOrEmpty(valor))
            {
                // Aplicar el filtro según el criterio seleccionado
                if (filtro == "marca")
                {
                    cargadores = cargadores.Where(c => c.Marca.StartsWith(valor));
                }
                else if (filtro == "ubicacion")
                {
                    cargadores = cargadores.Where(c => c.Ubicacion.StartsWith(valor));
                }
                else if (filtro == "potencia")
                {
                    // Convertir el valor de búsqueda a entero
                    int potencia = 0;
                    if (int.TryParse(valor, out potencia))
                    {
                        cargadores = cargadores.Where(c => c.Potencia.ToString().StartsWith(valor));
                    }
                }
            }

            // Obtener los cargadores paginados
            var cargadoresPaginados = cargadores.ToList();

            // Devolver los datos de cargadores y la información de paginación como JSON
            return Json(new { cargadores = cargadoresPaginados });
        }


        // Acción para registrar un nuevo cargador
        [HttpPost]
        public IActionResult Registrar(Cargador cargador)
        {
            if (ModelState.IsValid)
            {
                // Obtener el último ID de cargador en la base de datos
                var ultimoId = _context.Cargadores.OrderByDescending(c => c.Id).Select(c => c.Id).FirstOrDefault();
                
                
                cargador.Id = ultimoId + 1;

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

            // Eliminar también las asignaciones relacionadas
            var asignaciones = _context.Asignaciones.Where(a => a.IdCargador == id);
            _context.Asignaciones.RemoveRange(asignaciones);

            // Eliminar el cargador de la base de datos
            _context.Cargadores.Remove(cargador);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
