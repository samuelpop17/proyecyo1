using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcProyectoVinilos.Models;
using ProyectoMvcVinilacion.Data;
using System.Data;

namespace MvcProyectoVinilos.Repositories
{
    public class ProductosRepository
    {
        private WebContext context;
        private DataTable tabla;
        private SqlConnection cn;
        private SqlCommand com;

        public ProductosRepository(WebContext context)
        {
            this.context = context;
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=VINILOS;User ID=sa;Password=MCSD2023;Encrypt=False;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
            this.tabla = new DataTable();
            string sql = "select * from Productos";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tabla);
        }

        public List<Producto> GetProductos()
        {
            var consulta= from datos in this.tabla.AsEnumerable() select datos;
            List<Producto> productos = new List<Producto>();
            foreach (var row in consulta)
            {
                Producto producto = new Producto
                {
                    ProductoId=row.Field<int>("ProductoId"),
                    Nombre = row.Field<string>("Nombre"),
                    Descripcion = row.Field<string>("Descripcion"),
                    Imagen = row.Field<string>("Imagen"),
                    CategoriaId = row.Field<int>("CategoriaId"),
                    Precio = row.Field<decimal>("Precio"),
                };

                productos.Add(producto);
            }
            return productos;
        }

        public Producto FindProducto(int id)
        {
            var consulta= from datos in this.tabla.AsEnumerable() where datos.Field< int >("ProductoId") ==id select datos;
            var row = consulta.FirstOrDefault();
            Producto producto = new Producto
            {
                ProductoId = row.Field<int>("ProductoId"),
                Nombre = row.Field<string>("Nombre"),
                Descripcion = row.Field<string>("Descripcion"),
                Imagen = row.Field<string>("Imagen"),
                CategoriaId = row.Field<int>("CategoriaId"),
                Precio = row.Field<decimal>("Precio"),
            };
            return producto;
        }

        public async Task<List<Producto>> GetCategoriasAsync(string nombreCategoria)
        {
            var nombreCategoriaParam = new SqlParameter("@NombreCategoria", nombreCategoria);

            var categorias = await context.Productos
                .FromSqlRaw("EXECUTE SP_PRODUCTOS_POR_CATEGORIA @NombreCategoria", nombreCategoriaParam)
                .ToListAsync();

            return categorias;
        }


        public List<Categoria> GetCategorias()
        {
            var categorias = this.context.Categorias.ToList();
            return categorias;
        }

       

    }
}
