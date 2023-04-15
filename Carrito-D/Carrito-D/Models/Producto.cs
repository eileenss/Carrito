namespace Carrito_D.Models
{
    public class Producto
    {
        //atributos
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public double PrecioVigente { get; set; }
        public Boolean Activo { get; set; }
        public int Id { get; set; }
        public Categoria Categoria { get; set; }


    }
}
