using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcProyectoVinilos.Models;
using MvcProyectoVinilos.Repositories;
using ProyectoMvcVinilacion.Models;
using System.Security.Claims;

namespace MvcProyectoVinilos.Controllers
{
    public class AccountController : Controller
    {

        private UsuarioRepository repo;

        public AccountController(UsuarioRepository repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(string email, string contraseña)
        {
            Usuario usuario = this.repo.LogInUsuario(email, contraseña);

            if (usuario == null)
            {
                
                return View();
            }
            else
            {
                var datos = new List<Claim>
                {
                    new Claim("name",usuario.Nombre!),
                    new Claim("id",usuario.IdUsuario.ToString()),
                    new Claim("rol",usuario.Rol.ToString())
                };
                ClaimsIdentity identidad = new(datos, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties propiedades = new()
                {
                    IsPersistent = false,
                    AllowRefresh = true,
                    ExpiresUtc =DateTimeOffset.UtcNow.AddDays(1) 

                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad), propiedades);
                ViewData["INICIADO"] = "HAS INICIADO SESION ";
                return RedirectToAction("Index", "Home");
                
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }



        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Usuario usu)
        {

           bool registrado= this.repo.Registro(usu.Contraseña, usu.Email,usu.Nombre,usu.Apellidos);
            if (registrado)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewData["MENSAJE"] = "Error al registrar el usuario";
                return View();
            }
            
        }

        public IActionResult Perfil(int id)
        {
            Usuario usu = this.repo.FindUsuario(id);
            return View(usu);
        }

        public IActionResult Update(int id)
        {
            Usuario usu = this.repo.FindUsuario(id);
            return View(usu);
        }
        [HttpPost]
        public IActionResult Update(string nombre, string apellido, string email, string contraseña, int id)
        {
             this.repo.EditarUusario( nombre,  apellido,  email,  contraseña,  id);
            ViewData["MENSAJE"] = "Se edito correctamente";
            
            return View();
        }

        public IActionResult Sugerencias()
        {
            List<Sugerencia> suge=this.repo.GetSugerencias();
            return View(suge);
        }

    }
}
