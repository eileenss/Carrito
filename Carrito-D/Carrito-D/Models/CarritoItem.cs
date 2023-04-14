namespace Carrito_D.Models
{
    public class CarritoItem
    {
        public Carrito Carrito { get; set; }
        public Producto Producto { get; set; }
        public double ValorUnitario { get; set; }
        public int Cantidad { get; set; }
        public double Subtotal { get; set; }
    }
}
