using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.Models
{
    public class Persona
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(100000, 99999999, ErrorMessage = ErrorMsg.Rango)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:00.000.000")]
        public int DNI { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 2, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = ErrorMsg.CantCaracteres)]
        [DataType(DataType.Password)]
        [Display (Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Apellido { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int Telefono { get; set; }

        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Correo elctrónico")]
        public string Email { get; set; }

        //[Required(ErrorMessage = ErrorMsg.Requerido)]
        [Display (Name = "Fecha de alta")]
        [DataType(DataType.DateTime)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

    }
}
