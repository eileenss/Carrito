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
        [StringLength(100, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        //[RegularExpression(@"^(?:(?:00)?549?)?0?(?:11|[23]\d)(?:(?=\d{0,2}15)\d{2})??\d{8}$", ErrorMessage = ErrorMsg.Invalido)]
        [RegularExpression(@"^(549?)?(11\d{8})|([23]\d{9})$", ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Teléfono")]
        public int? Telefono { get; set; }

        [EmailAddress(ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Display(Name = "Stock")]
        public List<StockItem> StockItems { get; set; }

        public List<Compra> Compras { get; set; }
        

    }
}
