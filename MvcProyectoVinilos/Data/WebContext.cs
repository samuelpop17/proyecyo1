using Microsoft.EntityFrameworkCore;
using MvcProyectoVinilos.Models;
using ProyectoMvcVinilacion.Models;
using System.Collections.Generic;

namespace ProyectoMvcVinilacion.Data
{
    public class WebContext : DbContext
    {

        public WebContext(DbContextOptions<WebContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        public DbSet<Sugerencia> Sugerencias { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
    }
}