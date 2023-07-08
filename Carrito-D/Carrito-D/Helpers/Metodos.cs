using Carrito_D.Data;
using System.Security.Claims;


namespace Carrito_D.Helpers
{
    public class Metodos
    {
      public static string UrlPathFoto(string root, string nombre, string nombreDefault)
        {
            string url = string.Concat(root, string.IsNullOrEmpty(nombre)?nombreDefault:nombre);
            return url;
        }
    }
}
