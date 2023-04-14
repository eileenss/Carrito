namespace Carrito_D.Models
{
    public class Cliente : Persona
    {
        public int DNI { get; set; }
        public List<Compra> Compras { get; set; }
        public List<Carrito> Carritos { get; set; }

    }
}
