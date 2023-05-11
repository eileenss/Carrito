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

        //[Required(ErrorMessage = ErrorMsg.Requerido)] lo agregaremos nosotros con lógica
        //[Range(50, double.MaxValue, ErrorMessage = ErrorMsg.Rango)]
        //[DataType(DataType.Currency)]
        [NotMapped]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
        public decimal Total { get; set; }

        //[Required(ErrorMessage = ErrorMsg.Requerido)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Fecha { get; set; }  = DateTime.Now; //que se agregue automaticamente al crearse

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Sucursal")]
        public int SucursalId { get; set; }

        [Display(Name = "Sucursal de retiro")] 
        public Sucursal Sucursal { get; set; }

    }
}
