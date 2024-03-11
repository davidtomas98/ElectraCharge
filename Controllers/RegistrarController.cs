using Microsoft.AspNetCore.Mvc;
using ElectraCharge.Models;
using System.Linq;
using System;

namespace ElectraCharge.Controllers
{
    public class RegistrarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para mostrar el formulario de registro con la lista de usuarios y cargadores
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            var cargadores = _context.Cargadores.ToList();

            ViewBag.Usuarios = usuarios;
            ViewBag.Cargadores = cargadores;

            return View();
        }

        
        // Método para obtener la lista de asignaciones paginada y ordenada por fecha
        public IActionResult Index2(int pagina = 1, int cantidadPorPagina = 12)
        {
            // Calcular el índice de inicio y fin para la paginación
            int indiceInicio = (pagina - 1) * cantidadPorPagina;

            // Obtener la lista de asignaciones ordenada por fecha
            var asignacionesOrdenadas = _context.Asignaciones.OrderByDescending(a => a.Fecha);

            // Obtener la lista de asignaciones para la página actual
            var asignacionesPagina = asignacionesOrdenadas.Skip(indiceInicio).Take(cantidadPorPagina).ToList();

            // Devolver las asignaciones y la información de paginación como JSON
            return Json(new { Asignaciones = asignacionesPagina, PaginaActual = pagina, TotalPaginas = (int)Math.Ceiling((double)asignacionesOrdenadas.Count() / cantidadPorPagina) });
        }

        // Método para mostrar la lista de asignaciones con paginación
        public IActionResult ListarAsignaciones(int pagina = 1)
        {
            int cantidadPorPagina = 25;

            // Obtener la lista de asignaciones
            var asignaciones = _context.Asignaciones.ToList();

            // Calcular el total de páginas
            int totalAsignaciones = asignaciones.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalAsignaciones / cantidadPorPagina);

            // Asegurarse de que la página solicitada esté dentro del rango
            pagina = Math.Max(1, Math.Min(pagina, totalPaginas));

            // Obtener las asignaciones para la página actual
            var asignacionesPagina = asignaciones.Skip((pagina - 1) * cantidadPorPagina).Take(cantidadPorPagina).ToList();

            // Pasar las asignaciones y la información de paginación a la vista
            ViewBag.Asignaciones = asignacionesPagina;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View();
        }

        // Método para registrar una nueva asignación
        [HttpPost]
        public IActionResult Index(int idUsuario, int idCargador, int tiempo)
        {
            if (ModelState.IsValid)
            {
                // Obtener el último ID de asignación en la base de datos
                var ultimoId = _context.Asignaciones.OrderByDescending(a => a.Id).Select(a => a.Id).FirstOrDefault();
                
                // Convertir los minutos ingresados a un TimeSpan
                TimeSpan minutos = TimeSpan.FromMinutes(tiempo);

                var asignacion = new Asignacion
                {
                    Id = ultimoId + 1, 
                    IdUsuario = idUsuario,
                    IdCargador = idCargador,
                    Tiempo = minutos,
                    Fecha = DateTime.Now
                };

                _context.Asignaciones.Add(asignacion);
                _context.SaveChanges();
                return RedirectToAction("Index", "Registrar");
            }
            return View();
        }

        // Acción para filtrar asignaciones según el criterio de búsqueda
        [HttpGet]
        public IActionResult FiltrarAsignaciones(string filtro, string valor)
        {
            IQueryable<Asignacion> asignacionesFiltradas;

            // Verificar si se proporciona un valor de búsqueda
            if (!string.IsNullOrEmpty(valor))
            {
                // Realizar la operación de unión (join) para obtener el nombre del usuario
                asignacionesFiltradas = (from asignacion in _context.Asignaciones
                                        join usuario in _context.Usuarios on asignacion.IdUsuario equals usuario.Id
                                        where usuario.Nombre.StartsWith(valor)
                                        select asignacion);
            }
            else
            {
                // Si no se proporciona un valor de búsqueda, mostrar todas las asignaciones
                asignacionesFiltradas = _context.Asignaciones;
            }

            // Convertir la consulta en una lista
            var asignaciones = asignacionesFiltradas.ToList();

            // Devolver los resultados en formato JSON
            return Json(new { asignaciones = asignaciones });
        }

    }

}
