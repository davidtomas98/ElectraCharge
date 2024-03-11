using Microsoft.AspNetCore.Mvc;
using ElectraCharge.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ElectraCharge.Controllers
{
    /// <summary>
    /// Controlador para manejar la autenticación de usuarios.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna la vista de inicio de sesión.
        /// </summary>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Acción para manejar la solicitud de inicio de sesión.
        /// </summary>
        [HttpPost]
        public IActionResult Login(Administrador model, string returnUrl)
        {
                // Busca el usuario en la base de datos
                var user = _context.Administradores.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    // Crea una identidad para el usuario
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Autentica al usuario
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Redirige al returnUrl después del inicio de sesión exitoso
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        // Redirige a la página principal si no hay returnUrl o si es una URL no local
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Muestra mensaje de advertencia si el correo electrónico o la contraseña no coinciden
                    ModelState.AddModelError("Email", "Correo electrónico o contraseña incorrectos.");
                }
            

            // Si hay un error en el modelo o la autenticación falla, vuelve a mostrar el formulario de inicio de sesión
            return View(model);
        }

        /// <summary>
        /// Retorna la vista de registro.
        /// </summary>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Acción para manejar la solicitud de registro.
        /// </summary>
        [HttpPost]
        public IActionResult Register(Administrador model)
        {
            if (ModelState.IsValid)
            {
                // Verifica si el correo electrónico ya existe en la base de datos
                var existingAdmin = _context.Administradores.FirstOrDefault(a => a.Email == model.Email);
                if (existingAdmin != null)
                {
                    // Muestra un mensaje de error si el correo electrónico ya está registrado
                    ModelState.AddModelError("Email", "El correo electrónico ya está registrado.");
                    return View(model);
                }

                // Crea una nueva instancia de Administrador con los datos del modelo
                var admin = new Administrador
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password
                };

                // Agrega el administrador a la base de datos
                _context.Administradores.Add(admin);
                _context.SaveChanges();

                // Muestra mensaje de registro exitoso
                TempData["RegistroExitoso"] = "Registro exitoso.";

                // Redirige a la página principal después del registro exitoso
                return RedirectToAction("Index", "Home");
            }

            // Si el modelo no es válido, vuelve a mostrar el formulario de registro con los errores
            return View(model);
        }

        /// <summary>
        /// Acción para manejar la solicitud de cierre de sesión.
        /// </summary>
        public IActionResult Logout()
        {
            // Cierra la sesión
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige a la página principal después de cerrar sesión
            return RedirectToAction("Index", "Home");
        }
    }
}
