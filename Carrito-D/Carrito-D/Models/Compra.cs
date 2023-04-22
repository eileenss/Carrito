namespace Carrito_D.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }
        public float Total { get; set; }
        public DateTime Fecha { get; set; }
        public int SucursalId { get; set; } 
        public Sucursal Sucursal { get; set; }

    }
}
