using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carrito_D.Models
{
    public class CarritoItem
    {
        [ForeignKey("Carrito")]
        public int CarritoId { get; set; }

        [ForeignKey("Producto")]
        public int ProductoId { get; set; }

        public Carrito Carrito { get; set; }

        public Producto Producto { get; set; }

        //[DataType(DataType.Currency)]
        [NotMapped]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
        [Display(Name = "Precio unitario")]
        public decimal ValorUnitario { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMsg.Minimo)]
        public int Cantidad { get; set; }

        //[Required(ErrorMessage = ErrorMsg.Requerido)] lo agregamos nosotros con lógica 
        //[DataType(DataType.Currency)]
        [NotMapped]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
        public decimal Subtotal { get; set; }  

    }
}
