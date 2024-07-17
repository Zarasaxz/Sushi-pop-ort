using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G2.Models
{
    public class Reclamo
    {
        public int Id { get; set; }
        [MaxLength(255,ErrorMessage = "Nombre completo no puede contener más de 255 caracteres")]
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public string Email { get; set; }
        [MaxLength(10, ErrorMessage = "Teléfono no puede contener más de 10 caracteres")]
        [Required]
        public string Telefono { get; set; }
        [MinLength(50,ErrorMessage = "Detalle de reclamo no puede contener menos de 50 caracteres")]
        [MaxLength(int.MaxValue, ErrorMessage = "Detalle de reclamo no puede contener más de 50 caracteres")]
        public string DetalleReclamo { get; set; }

        // Relacion 1 a 1 con Pedido
        [Required]
        public int PedidoId { get; set; } // FK de modelo Pedido
        public Pedido? Pedido { get; set; } // Objeto por que Reclamo tiene la FK

    }
}
