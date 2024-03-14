using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProyectoVinilos.Models
{
    [Table("Categorias")]
    public class Categoria
    {

        [Key]
        [Column("CATEGORIAID")]
        public int CategoriaId { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
    }
}
