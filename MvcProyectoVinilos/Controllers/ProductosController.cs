using Microsoft.AspNetCore.Mvc;
using MvcProyectoVinilos.Extension;
using MvcProyectoVinilos.Models;
using MvcProyectoVinilos.Repositories;

namespace MvcProyectoVinilos.Controllers
{
    public class ProductosController : Controller
    {
        private ProductosRepository repo;
        private CaritoRepository repo2;

        public ProductosController(ProductosRepository repo, CaritoRepository repo2)
        {
            this.repo = repo;
            this.repo2 = repo2;

        }

        public async Task<IActionResult> Index(int? id, string nombreCategoria)
        {
            if (id != null)
            {
                List<int> productosList = HttpContext.Session.GetObject<List<int>>("PRODUCTOS") ?? new List<int>();
                productosList.Add(id.Value);
                HttpContext.Session.SetObject("PRODUCTOS", productosList);
                ViewData["MENSAJE"] = "Producto almacenado correctamente";
            }

            // Obtener las categorías
            List<Categoria> cat = this.repo.GetCategorias();
            ViewBag.Categorias = cat;

            // Filtrar los productos por categoría si se ha seleccionado una categoría
            List<Producto> productos;
            if (nombreCategoria!=null)
            {
                productos = await this.repo.GetCategoriasAsync(nombreCategoria);
            }
            else
            {
                // Obtener todos los productos si no se ha seleccionado una categoría
                productos = this.repo.GetProductos();
            }

            return View(productos);
        }




        public IActionResult Details(int id)
        {
            Producto pro = this.repo.FindProducto(id);
            return View(pro);
        }


     

        public async Task<IActionResult> CarritoList(int? idEliminar)
        {
            List<int> idsProductos = HttpContext.Session.GetObject<List<int>>("PRODUCTOS");
            if (idsProductos != null)
            {
                if (idEliminar != null)
                {
                    idsProductos.Remove(idEliminar.Value);
                    HttpContext.Session.SetObject("PRODUCTOS", idsProductos);
                }
                List<Producto> productos = await repo2.GetFavoritosAsync(idsProductos);
                return View(productos);
            }
            return View();
        }
    }
}
