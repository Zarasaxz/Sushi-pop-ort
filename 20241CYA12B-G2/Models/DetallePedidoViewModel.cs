namespace _20241CYA12B_G2.Models
{
    public class DetallePedidoViewModel
    {
        public int identificadorCarrito {  get; set; }
        public string Cliente { get; set; }
        public string Direccion { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GastoEnvio { get; set; }
        public decimal Total { get; set; }
        public List<string> Productos { get; set; }

    }
}
