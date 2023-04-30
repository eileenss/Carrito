using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        
        [DataType(DataType.Currency)]
        public decimal Subtotal { get; set; }

        public Compra Compra { get; set; }

    }
}
