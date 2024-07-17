using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G2.Models
{
    public class Producto
    {
        public int Id { get; set; }
        [MaxLength(100,ErrorMessage = "Nombre no puede contener más de 100 caracteres")]
        [Required]
        public string? Nombre { get; set; }
        [MinLength(5,ErrorMessage = "Descripción no puede contener menos de 5 caracteres")]
        [MaxLength(250,ErrorMessage = "Descripción no puede contener más de 250 caracteres")]
        [Required]
        public string? Descripcion { get; set; }
        [Required]
        public decimal Precio { get; set;}
        public string? Foto { get; set; }
        [DefaultValue(100)]
        [Required]
        public int Stock { get; set; }
        [Required]
        public decimal Costo { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        public ICollection<Descuento>? Descuentos { get; set; }
        public ICollection<CarritoItem>? CarritoItems { get; set; }
    }
}
