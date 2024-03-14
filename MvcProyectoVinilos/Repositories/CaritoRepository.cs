using Microsoft.EntityFrameworkCore;
using MvcProyectoVinilos.Models;
using ProyectoMvcVinilacion.Data;

namespace MvcProyectoVinilos.Repositories
{
    public class CaritoRepository
    {
        private WebContext context;

        public CaritoRepository(WebContext context)
        {
            this.context = context;
        }

        public async Task<List<Producto>> GetProductoAsync()
        {
            return this.context.Productos.ToList();
        }
        public async Task<Producto> FindProducto(int id)
        {
            return await this.context.Productos.FirstOrDefaultAsync(z => z.ProductoId == id);
        }

        public async Task<List<Producto>> GetFavoritosAsync(List<int> ids)
        {
            var consulta = from datos in this.context.Productos where ids.Contains(datos.ProductoId) select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                return await consulta.ToListAsync();
            }
        }
    }
}
