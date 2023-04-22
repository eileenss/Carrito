using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carrito_D.Models
{
    public class CarritoItem
    {
        [ForeignKey("Producto")]
        public int ProductoId { get; set; }

        [ForeignKey("Carrito")]
        public int CarritoId { get; set; }

        public Carrito Carrito { get; set; }

        public Producto Producto { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(50, float.MaxValue, ErrorMessage = ErrorMsg.Rango)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:$}")]
        [Display(Name = "Precio unitario")]
        public float ValorUnitario { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(1, 500, ErrorMessage = ErrorMsg.Rango)]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(50, float.MaxValue, ErrorMessage = ErrorMsg.Rango)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:$}")]
        public float Subtotal { get; set; }

    }
}
