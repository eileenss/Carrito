using Carrito_D.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Carrito_D.Models
{
    public class Carrito
    {
        public int Id { get; set; }

        [Display(Name = "Carrito activo")]
        public bool Activo { get; set; } = true;

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }  

        public Cliente Cliente { get; set; }

        [Display(Name = "Items")]
        public List<CarritoItem> CarritoItems { get; set; }

        //[DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
        public decimal Subtotal { get; set; }

        public Compra Compra { get; set; }

    }
}
