using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Carrito_D.ViewModels
{
    public class EditPersona
    {
        public int Id { get; set; } 

        [RegularExpression(@"^(549?)?(11\d{8})|([23]\d{9})$", ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Teléfono")]
        public int? Telefono { get; set; }

        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
    }
}
