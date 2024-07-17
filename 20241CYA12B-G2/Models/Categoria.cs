using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G2.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "Nombre no debe contener más de 100 caracteres")]
        [Required (ErrorMessage = "Nombre es obligatorio")]
        public string Nombre { get; set; }
        [MaxLength(int.MaxValue)]
        public string? Descripcion { get; set;}
        public ICollection<Producto>? Productos { get; set; }
    }
}
