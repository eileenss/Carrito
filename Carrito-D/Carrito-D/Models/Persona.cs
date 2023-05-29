using Carrito_D.Helpers;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.Models
{
    public class Persona : IdentityUser<int>
    {
        //public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [RegularExpression(@"^\d{1,2}\.?\d{3}\.?\d{3}$", ErrorMessage = ErrorMsg.Invalido)]
        public string DNI { get; set; }

        /*[Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 2, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }*/

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = ErrorMsg.CantCaracteres)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Apellido { get; set; }

        //[RegularExpression(@"^(?:(?:00)?549?)?0?(?:11|[23]\d)(?:(?=\d{0,2}15)\d{2})??\d{8}$", ErrorMessage = ErrorMsg.Invalido)]
        [RegularExpression(@"^(549?)?(11\d{8})|([23]\d{9})$", ErrorMessage = ErrorMsg.Invalido)] //creada
        [Display(Name = "Teléfono")]
        public int? Telefono { get; set; }

        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = ErrorMsg.CantCaracteres)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Correo electrónico")]
        public override string Email
        {
            get { return base.Email; }
            set { base.Email = value; }
        }

        [Display (Name = "Fecha de registro")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaAlta { get; set; } = DateTime.Now;

    }
}
