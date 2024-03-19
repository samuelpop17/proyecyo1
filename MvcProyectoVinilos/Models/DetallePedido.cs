using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProyectoVinilos.Models
{
    [Table("DetallePedido")]
    public class DetallePedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("DetallePedidoId")]
        public int DetallePedidoId { get; set; }

        [Column("PedidoId")]
        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public virtual Pedido Pedido { get; set; }

        [Column("ProductoId")]
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; } // Asume que tienes un modelo Producto

        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Column("Precio", TypeName = "decimal(18, 2)")]
        public decimal Precio { get; set; }
    }
}
