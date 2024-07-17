using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G2.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        [DefaultValue(false)]
        [Required]
        public bool Procesado { get; set; }
        [DefaultValue(false)]
        [Required]
        public bool Cancelado { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public virtual Pedido? Pedido { get; set; }
        public ICollection<CarritoItem>? CarritoItems { get; set; }


    }
}
