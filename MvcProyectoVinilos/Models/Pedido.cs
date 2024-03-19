using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProyectoVinilos.Models
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PedidoId")]
        public int PedidoId { get; set; }

        [Column("FechaPedido")]
        public DateTime FechaPedido { get; set; }

        [Column("UsuarioId")]
        public string UsuarioId { get; set; } // Asegura que el tipo de dato coincida con tu esquema de usuarios

        // Relación uno a muchos con DetallePedido
        public virtual List<DetallePedido> Detalles { get; set; }
    }
}
