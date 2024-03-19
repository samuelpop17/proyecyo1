using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcProyectoVinilos.Extension;
using MvcProyectoVinilos.Models;
using MvcProyectoVinilos.Repositories;
using ProyectoMvcVinilacion.Data;
using System.Security.Claims;

namespace MvcProyectoVinilos.Controllers
{
    public class ProductosController : Controller
    {
        private ProductosRepository repo;
        private CaritoRepository repo2;
        private WebContext context;

        public ProductosController(ProductosRepository repo, CaritoRepository repo2, WebContext context)
        {
            this.repo = repo;
            this.repo2 = repo2;
            this.context = context;

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


        public async Task<IActionResult> RealizarCompra()
        {
            var cantidades = HttpContext.Session.GetObject<Dictionary<int, int>>("PRODUCTOS_CANTIDADES");

            if (!User.Identity.IsAuthenticated)
            {
                // Redirigir al usuario a la página de inicio de sesión
                return RedirectToAction("Login", "Account");
            }

            var idsProductos = HttpContext.Session.GetObject<List<int>>("PRODUCTOS");
            if (idsProductos == null || !idsProductos.Any() && cantidades == null || !cantidades.Any())
            {
                // No hay productos en el carrito, redirigir al índice
                return RedirectToAction("Index", "Productos");
            }

            List<Producto> productosEnCarrito = await repo2.GetFavoritosAsync(idsProductos);

            Pedido nuevoPedido = new Pedido
            {
                FechaPedido = DateTime.Now,
                UsuarioId = User.FindFirstValue("id"), // Asegúrate de que esto no sea null
                Detalles = productosEnCarrito.Select(p => new DetallePedido
                {
                    ProductoId = p.ProductoId,
                    Cantidad = cantidades[p.ProductoId], // Asume una cantidad fija o ajusta según tu lógica
                    Precio = p.Precio
                }).ToList()
            };

            context.Pedidos.Add(nuevoPedido);
            await context.SaveChangesAsync(); // Guarda los cambios en la base de datos

            // Limpia el carrito
            HttpContext.Session.Remove("PRODUCTOS");

            return RedirectToAction("ConfirmacionPedido", new { id = nuevoPedido.PedidoId });
        }



        public IActionResult ConfirmacionPedido(int id)
        {
            // Verificar si el id es 0, lo cual puede indicar que no se ha creado el pedido correctamente.
            if (id <= 0)
            {
                // Podrías redireccionar a una página de error o a la lista de pedidos
                return RedirectToAction("Index", "Productos");
            }

            // Buscar el pedido por id y pasarlo a la vista
            var pedido = context.Pedidos.Include(p => p.Detalles)
                                        .ThenInclude(d => d.Producto)
                                        .FirstOrDefault(p => p.PedidoId == id);

            if (pedido == null)
            {
                // Si el pedido no existe, incluso después de pasar un id válido, manejar adecuadamente.
                return NotFound();
            }

            return View(pedido);
        }

        [HttpPost]
        public IActionResult ActualizarCarrito(Dictionary<int, int> cantidades, string accion)
        {
            if (accion.StartsWith("eliminar-"))
            {
                int idProducto = int.Parse(accion.Split('-')[1]);
                // Elimina el producto del carrito
                // Asegúrate de actualizar la sesión del carrito adecuadamente
            }
            else if (accion == "comprar")
            {
                // Aquí puedes redirigir a otra acción que maneje la compra
                // Pasar las cantidades como parte del redireccionamiento si es necesario
                // Por ejemplo, podrías guardar las cantidades actualizadas en la sesión antes de redirigir
                HttpContext.Session.SetObject("PRODUCTOS_CANTIDADES", cantidades);
                return RedirectToAction("RealizarCompra");
            }
            return RedirectToAction("CarritoList");
        }

        public async Task<IActionResult> VistaPedidos(int id)
        {
            List<Pedido> pedidosusu=await this.repo.GetPedidos(id);
            return View(pedidosusu);
        }

        public async Task<IActionResult> VistaPedidosDetalles(int id)
        {
            var detallesPedido = await repo.GetPedidosDetalles(id);

            if (detallesPedido == null || !detallesPedido.Any())
            {
                // Maneja el caso donde no hay detalles para el pedido
                // Esto podría redirigir al usuario a una página de error o de vuelta a la lista de pedidos
                return RedirectToAction("VistaPedidos");
            }

            return View(detallesPedido);
        }

        public async Task<IActionResult> EliminarPedido(int id)
        {
            // Encuentra el pedido que coincida con el id proporcionado
            var pedido = await context.Pedidos
                                      .Include(p => p.Detalles) // Asegúrate de incluir los detalles del pedido
                                      .FirstOrDefaultAsync(p => p.PedidoId == id);

            if (pedido != null)
            {
                // Elimina los detalles del pedido primero para evitar violaciones de restricciones de clave foránea
                context.DetallesPedido.RemoveRange(pedido.Detalles);

                // Ahora elimina el pedido
                context.Pedidos.Remove(pedido);

                // Guarda los cambios en la base de datos
                await context.SaveChangesAsync();

                // Puedes añadir aquí lógica adicional si necesitas, como actualizar el inventario o enviar notificaciones

                ViewData["Mensaje"] = "Pedido cancelado con éxito.";
            }
            else
            {
                ViewData["Error"] = "Pedido no encontrado.";
            }
            var userId = User.FindFirstValue("id");
            // Redirige al usuario a su lista de pedidos después de cancelar
            return RedirectToAction("VistaPedidos", new { id = userId });
        }



    }
}
