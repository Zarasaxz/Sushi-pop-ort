using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G2.Models
{
    public class Descuento
    {
        public int Id { get; set; }
        [Required]
        public int Dia { get; set; }
        [DefaultValue(0)]
        [Required]
        public int Porcentaje { get; set; }
        [DefaultValue(1000)]
        public decimal DescuentoMaximo { get; set; }
        [DefaultValue(true)]
        [Required]
        public bool Activo { get; set; }
        /*CONSULTAR PROFESOR*/
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
    }
}
