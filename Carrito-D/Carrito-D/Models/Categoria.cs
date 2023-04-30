using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Nombre { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = ErrorMsg.CantCaracteres)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public List<Producto> Productos { get; set; }  


    }
}
