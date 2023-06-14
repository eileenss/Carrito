using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.ViewModels
{
    public class CrearEmpleado
    {
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [RegularExpression(@"^\d{1,2}\.?\d{3}\.?\d{3}$", ErrorMessage = ErrorMsg.Invalido)]
        public string DNI { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Apellido { get; set; }

        [RegularExpression(@"^(549?)?(11\d{8})|([23]\d{9})$", ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Teléfono")]
        public int? Telefono { get; set; }

        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

    }
}
