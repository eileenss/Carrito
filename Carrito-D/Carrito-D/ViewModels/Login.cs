using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Carrito_D.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        public bool Recordarme { get; set; }
    }
}
