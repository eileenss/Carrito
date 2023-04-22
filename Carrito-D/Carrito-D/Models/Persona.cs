namespace Carrito_D.Models
{
    public class Persona
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [Range(100000, 99999999), ErrorMessage = ErrorMsg.Rango]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:00.000.000")]
        public int DNI { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 2, ErrorMessage = ErrorMsg.CantCaracteres)]
        public string UserName { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 2, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.Requerido)]
        [StringLength(85, MinimumLength = 2, ErrorMessage = ErrorMsg.CantCaracteres)]
        [RegularExpression(@"[a-zA-Z áéíóú]*", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Apellido { get; set; }

        public int Telefono { get; set; }
        
        public string Direccion { get; set; }
        
        public string Email { get; set; }

        public DateTime FechaAlta { get; set; } = DateTime.Now;

    }
}
