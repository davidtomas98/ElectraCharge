using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ElectraCharge.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElectraCharge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Método para mostrar la página de inicio
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // Método para mostrar la página de privacidad
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        // Método para mostrar la página de acceso denegado
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Método para manejar errores
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Mostrar la vista de error con un modelo que contiene el ID de la solicitud
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
