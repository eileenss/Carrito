using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.Models
{
    public class Cliente : Persona
    {
        [RegularExpression(@"[0-9]{11}", ErrorMessage = ErrorMsg.SoloNums)]
        [Display(Name = "CUIL/CUIT")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:00-00000000-0")]
        public int Cuil { get; set; }

        public List<Compra> Compras { get; set; }

        public List<Carrito> Carritos { get; set; }

    }
}


