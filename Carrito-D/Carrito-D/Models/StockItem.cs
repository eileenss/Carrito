using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.Models
{
    public class StockItem
    {
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Producto")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [ForeignKey("Sucursal")]
        public int SucursalId { get; set; }
        public Sucursal Sucursal { get; set; }
        public Producto Producto { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(0, 500, ErrorMessage = ErrorMsg.Rango)]
        public int Cantidad { get; set; }

    }
}
