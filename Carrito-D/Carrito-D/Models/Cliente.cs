using Carrito_D.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Carrito_D.Models
{
    public class Cliente : Persona
    {
        [RegularExpression(@"^(20|23|24|27|30|33)(-|\s)?\d{8}(-|\s)?\d{1}$", ErrorMessage = ErrorMsg.Invalido)]
        [Display(Name = "Número de identificación tributaria")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:00-00000000-0")]
        public string Cuil { get; set; }

        public List<Compra> Compras { get; set; }

        public List<Carrito> Carritos { get; set; }

    }
}


