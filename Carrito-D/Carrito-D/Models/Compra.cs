using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carrito_D.Models
{
    public class Compra
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Carrito")]
        public int CarritoId { get; set; }

        public Carrito Carrito { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(50, float.MaxValue, ErrorMessage = ErrorMsg.Rango)]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Sucursal")]
        public int SucursalId { get; set; } 

        public Sucursal Sucursal { get; set; }

    }
}
