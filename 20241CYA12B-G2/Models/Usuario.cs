using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G2.Models
{
    public abstract class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo nombre es obligario")]
        [MinLength(5, ErrorMessage = "Escribir 5 caracteres al menos")]
        [MaxLength(30, ErrorMessage = "No puede tener mas de 30 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(5, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(30, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string Apellido { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MaxLength(100, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MinLength(10, ErrorMessage = ErrorViewModel.CaracteresMinimos)]
        [MaxLength(10, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string Telefono { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Fecha de alta")]
        public DateTime? FechaAlta { get; set; }
        public bool Activo { get; set; } = true;

        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = ErrorViewModel.CampoRequerido)]
        [MaxLength(100, ErrorMessage = ErrorViewModel.CaracteresMaximos)]
        public string Email { get; set; }
    }
}
