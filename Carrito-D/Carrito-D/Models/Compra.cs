namespace Carrito_D.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public Carrito Carrito { get; set; }
        public float Total { get; set; }
        public DateTime Fecha { get; set; }
        public Sucursal Sucursal { get; set; }

    }
}
