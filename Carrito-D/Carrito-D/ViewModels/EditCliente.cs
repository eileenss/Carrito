using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Carrito_D.ViewModels
{
    public class EditCliente
    {
        public int Id { get; set; }

        [RegularExpression(@"^(20|23|24|27|30|33)(-|\s)?\d{8}(-|\s)?\d{1}$", ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Número de identificación tributaria")]
        public string Cuil { get; set; }

        [RegularExpression(@"^(549?)?(11\d{8})|([23]\d{9})$", ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Teléfono")]
        public int? Telefono { get; set; }

        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
    }
}
