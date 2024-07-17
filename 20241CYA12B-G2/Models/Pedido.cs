using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G2.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        [Required]
        public int NroPedido { get; set; }
        [Required]
        public DateTime FechaCompra { get; set; }
        [Required]
        public decimal Subtotal { get; set; }
        [Required]
        public decimal GastoEnvio { get; set; }
        [Required]
        public decimal Total { get; set; }
        /*default no, valores no*/
        [Required]
        public int Estado { get; set; }

        [Required]
        public int CarritoId { get; set; } // FK de modelo Carrito

        public Carrito? Carrito { get; set; }

        // Relacion 1 a 1 con Reclamo
        public virtual Reclamo? Reclamo { get; set; } // Virtual  por que la PK ID es la FK de Reclamo que es PedidoID


    }
}
