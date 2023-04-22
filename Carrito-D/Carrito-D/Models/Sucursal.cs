using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.Models
{
    public class Sucursal
    {
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        public string Direccion { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int Telefono { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        public List<StockItem> StockItems { get; set; }
        public List<Compra> Compras { get; set; }
        

    }
}
