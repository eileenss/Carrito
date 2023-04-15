namespace Carrito_D.Models
{
    public class Categoria
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Id { get; set; }
        public List <Producto> Productos { get; set; }  


    }
}
