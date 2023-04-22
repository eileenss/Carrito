using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.Models
{
    public class Sucursal
    {
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int Telefono { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Correo elctrónico")]
        public string Email { get; set; }

        [Display(Name = "Stock")]
        public List<StockItem> StockItems { get; set; }
        public List<Compra> Compras { get; set; }
        

    }
}
