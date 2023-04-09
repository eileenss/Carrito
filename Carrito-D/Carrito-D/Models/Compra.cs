namespace Carrito_D.Models
{
    public class Compra
    {
        public Cliente Cliente { get; set; }
        public Carrito Carrito { get; set; }
        public double Total { get; set; }
    }
}
