using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G2.Models
{
    public class Contacto
    {
        public int Id { get; set; }
        [MaxLength(255,ErrorMessage = "Nombre completo no puede contener más de 255 caracteres")]
        [Required(ErrorMessage = "Nombre completo es obligatorio")]
        public string NombreCompleto { get; set; }
        [Required]
        public string Email { get; set; }
        [MaxLength(10, ErrorMessage = "Teléfono no puede contener más de 10 caracteres")]
        [Required]
        public string Telefono { get; set; }
        [MaxLength(int.MaxValue)]
        public string Mensaje { get; set; }
        [DefaultValue(false)]
        [Required]
        public bool Leido { get; set; }
    }
}
