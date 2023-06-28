using Carrito_D.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Carrito_D.ViewModels
{
    public class Registro
    {
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Correo electrónico")]
        [Remote(action: "EmailExistente", controller: "Account")]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = ErrorMsg.NoCoincide)]
        public string ConfirmedPassword { get; set; }

        
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [RegularExpression(@"^\d{1,2}\.?\d{3}\.?\d{3}$", ErrorMessage = ErrorMsg.Invalido)]
        [Remote(action: "DniExistente", controller: "Account")]
        public string DNI { get; set; }


        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Apellido { get; set; }

    }
}
