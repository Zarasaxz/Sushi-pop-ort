namespace _20241CYA12B_G2.Models
{
    public class Cliente : Usuario
    {
        public int? NumeroCliente { get; set; }

        public ICollection<Carrito>? Carritos { get; set; }
        public ICollection<Reserva>? Reservas { get; set; }
    }
}
