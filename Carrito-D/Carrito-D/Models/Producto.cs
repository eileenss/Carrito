using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carrito_D.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 2, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú 0-9]*", ErrorMessage = ErrorMsg.Alfanumerico)]
        public string Nombre { get; set; }

        [StringLength(200, MinimumLength = 2, ErrorMessage = ErrorMsg.CantCaracteres)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public string Imagen { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(0, double.MaxValue, ErrorMessage = ErrorMsg.Rango)]
        [Display(Name = "Precio")]
        public decimal PrecioVigente { get; set; }

        [Display(Name = "Producto activo")]
        public bool Activo { get; set; } = true;

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }

        public List<StockItem> StockItems { get; set;}

        public List<CarritoItem> CarritoItems { get; set; }


    }
}
