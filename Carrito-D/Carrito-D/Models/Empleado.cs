using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.Models
{
    public class Empleado : Persona
    {
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(1000, 999999, ErrorMessage = ErrorMsg.Rango)]
        public int Legajo { get; set; }

    }
}
