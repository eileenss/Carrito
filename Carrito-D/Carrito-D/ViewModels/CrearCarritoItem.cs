using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.ViewModels
{
    public class CrearCarritoItem
    {
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMsg.Minimo)]
        public int Cantidad { get; set; }
    }
}
