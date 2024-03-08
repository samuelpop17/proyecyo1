using Microsoft.AspNetCore.Mvc;
using MvcProyectoVinilos.Models;
using MvcProyectoVinilos.Repositories;

namespace MvcProyectoVinilos.Controllers
{
    public class ProductosController : Controller
    {
        private ProductosRepository repo;

        public ProductosController(ProductosRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Producto> producto=this.repo.GetProductos();
            return View(producto);
        }

        public IActionResult Details(int id)
        {
            Producto pro = this.repo.FindProducto(id);
            return View(pro);
        }
    }
}
