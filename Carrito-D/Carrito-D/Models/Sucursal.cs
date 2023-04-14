namespace Carrito_D.Models
{
    public class Sucursal
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; }
        public List<StockItem> StockItems { get; set; }
        public int Id { get; set; }
    }
}
