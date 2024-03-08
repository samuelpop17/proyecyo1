using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProyectoVinilos.Models
{
    [Table("SUGERENCIAS")]
    public class Sugerencia
    {
        [Key]
        [Column("IDSUGERENCIA")]
        public int IdSugerencia { get; set; }

        [Column("EMAIL")]
        [Required(ErrorMessage = "El campo Email es obligatorio")]
        [EmailAddress(ErrorMessage = "Por favor, introduzca una dirección de correo electrónico válida")]
        public string Email { get; set; }

        [Column("NOMBRE")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Column("SUGERENCIA")]
        [Required(ErrorMessage = "El campo Sugerencia es obligatorio")]
        public string SugerenciaTexto { get; set; }
    }
}
