namespace Carrito_D.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        public bool Activo { get; set; }
        public Cliente Cliente { get; set; }
        public List<CarritoItem> CarritoItems { get; set; }
        public float Subtotal { get; set; }
    }
}
