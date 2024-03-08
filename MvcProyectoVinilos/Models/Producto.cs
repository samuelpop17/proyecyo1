using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcProyectoVinilos.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        [Column("ProductoId")]
        public int ProductoId { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Column("Imagen")]
        public string Imagen { get; set; }

        [Column("Precio")]
        public decimal Precio { get; set; }

        [Column("CategoriaId")]
        public int CategoriaId { get; set; }
    }
}
