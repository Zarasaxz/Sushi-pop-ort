using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G2.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }
        [Required]
        public decimal PrecioUnitarioConDescuento { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public int CarritoId { get; set; }

        public Carrito? Carrito { get; set; }
        [Required]
        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }


    }
}
