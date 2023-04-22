using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carrito_D.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 3, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Nombre { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = ErrorMsg.CantCaracteres)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public string Imagen { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(50, 300000, ErrorMessage = ErrorMsg.Rango)]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio")]
        public float PrecioVigente { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Display(Name = "Producto activo")]
        public bool Activo { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public List<StockItem> StockItems { get; set;}
        public List<CarritoItem> CarritoItems { get; set; }


    }
}
