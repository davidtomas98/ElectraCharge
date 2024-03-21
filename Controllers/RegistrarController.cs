using Microsoft.AspNetCore.Mvc;
using ElectraCharge.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

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
        [Authorize]
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            var cargadores = _context.Cargadores.ToList();

            ViewBag.Usuarios = usuarios;
            ViewBag.Cargadores = cargadores;

            return View();
        }

        
         
        // Método para obtener la lista de asignaciones paginada
        public IActionResult Index2(int pagina = 1, int cantidadPorPagina = 25)
        {
            // Calcular el índice de inicio y fin para la paginación
            int indiceInicio = (pagina - 1) * cantidadPorPagina;

            // Obtener la lista de asignaciones
            var asignaciones = _context.Asignaciones.ToList();

            // Obtener la lista de asignaciones para la página actual
            var asignacionPagina = asignaciones.Skip(indiceInicio).Take(cantidadPorPagina);

            // Devolver los asignaciones y la información de paginación como JSON
            return Json(new { Asignaciones = asignacionPagina, PaginaActual = pagina, TotalPaginas = (int)Math.Ceiling((double)asignaciones.Count / cantidadPorPagina) });
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

                // Enviar correo electrónico al usuario
                EnviarCorreoElectronico(idUsuario);

                // Establecer mensaje de éxito en TempData
                TempData["MensajeExito"] = "La asignación se ha realizado correctamente.";

                return RedirectToAction("Index", "Registrar");
            }

            return View();
}

        // Método para enviar correo electrónico al usuario
        private void EnviarCorreoElectronico(int idUsuario)
        {
            // Obtener la información del usuario
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == idUsuario);

            // Obtener la última asignación para el usuario con el ID especificado
            var ultimaAsignacion = _context.Asignaciones
                .Where(a => a.IdUsuario == idUsuario)
                .OrderByDescending(a => a.Fecha)
                .FirstOrDefault();

            if (usuario != null && ultimaAsignacion != null)
            {
                try
                {
                    // Obtener el nombre de usuario y la contraseña del administrador autenticado si la identidad no es nula
                    var adminUsername = User?.Identity?.Name;

                    if (adminUsername != null)
                    {
                        var admin = _context.Administradores.FirstOrDefault(a => a.Email == adminUsername);
                        if (admin != null)
                        {
                            // Crear un cliente de correo electrónico
                            SmtpClient clienteSmtp = new SmtpClient("smtp.outlook.com")
                            {
                                Port = 587,
                                Credentials = new System.Net.NetworkCredential(admin.Email, admin.Password),
                                EnableSsl = true
                            };

                            // Crear el mensaje de correo electrónico
                            MailMessage mensaje = new MailMessage(admin.Email, usuario.CorreoElectronico)
                            {
                                Subject = "Nueva asignación realizada",
                                Body = $"Hola {usuario.Nombre},\n\nSe ha realizado una nueva asignación para ti.\n\nTiempo asignado: {ultimaAsignacion.Tiempo} minutos.\n\nSaludos,\n{admin.Username}"
                            };

                            // Enviar el correo electrónico
                            clienteSmtp.Send(mensaje);
                        }
                        else
                        {
                            Console.WriteLine("No se encontró el administrador en la base de datos.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se pudo obtener el nombre de usuario del administrador autenticado.");
                    }
                }
                catch (Exception ex)
                {
                    // Manejar cualquier error en el envío del correo electrónico
                    Console.WriteLine("Error al enviar el correo electrónico: " + ex.ToString());
                }
            }
            else
            {
                Console.WriteLine("No se encontró información de usuario o asignación para el usuario con ID: " + idUsuario);
            }
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
