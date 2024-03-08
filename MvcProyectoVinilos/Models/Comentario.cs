using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcProyectoVinilos.Models
{
    [Table("Comentarios")]
    public class Comentario
    {
        [Key]
        [Column("IDCOMENTARIO")]
        public int IdComentario { get; set; }

        [Column("NOMBREUSUARIO")]
        public string NombreUusario { get; set; }

        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }

        [Column("COMENTARIO")]
        public string Coment { get; set; }
    }
}
