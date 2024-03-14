using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProyectoVinilos.Models;
using MvcProyectoVinilos.Repositories;
using System.Diagnostics;

namespace MvcProyectoVinilos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UsuarioRepository repo;

        public HomeController(ILogger<HomeController> logger,UsuarioRepository repo)
        {
            _logger = logger;
            this.repo = repo;
        }

       
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string email, string nombre, string sugerencia)
        {
            this.repo.InsertSugerencia(email, nombre, sugerencia);
            return View();
        }

        public IActionResult Contacto()
        {
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
