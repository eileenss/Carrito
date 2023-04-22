using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carrito_D.Models
{
    public class Carrito
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Display(Name = "Carrito activo")]
        public bool Activo { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }  

        public Cliente Cliente { get; set; }

        [Display(Name = "Items")]
        public List<CarritoItem> CarritoItems { get; set; }

        [Range(50, float.MaxValue, ErrorMessage = ErrorMsg.Rango)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:$}")]
        public float Subtotal { get; set; }

        public Compra Compra { get; set; }

    }
}
