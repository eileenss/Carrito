using Carrito_D.Helpers;
using Carrito_D.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Carrito_D.ViewModels
{
    public class CrearProducto
    {
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 2, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        public string Nombre { get; set; }

        [StringLength(200, MinimumLength = 2, ErrorMessage = ErrorMsg.CantCaracteres)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public IFormFile Imagen { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(0, double.MaxValue, ErrorMessage = ErrorMsg.Rango)]
        [Display(Name = "Precio")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
        public decimal PrecioVigente { get; set; }

        [Display(Name = "Producto activo")]
        public bool Activo { get; set; } = true;

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Categoria")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

    }
}
