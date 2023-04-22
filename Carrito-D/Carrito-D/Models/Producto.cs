namespace Carrito_D.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public float PrecioVigente { get; set; }
        public bool Activo { get; set; }
        public int CategoriaId { get; set; }    
        public Categoria Categoria { get; set; }
        public List<StockItem> StockItems { get; set;}
        public List<CarritoItem> CarritoItems { get; set; }


    }
}
