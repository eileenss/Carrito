using Carrito_D.Data;
using Carrito_D.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carrito_D.Controllers
{
    public class PrecargaController : Controller
    {
        private readonly CarritoContext _carritoContext;

        public PrecargaController(CarritoContext context)
        {
            this._carritoContext = context;
        }

        public IActionResult Seed() //precarga de datos
        {
            Cliente cliente = new Cliente() {
                Cuil = "20123456780",
                DNI = "1234567",
                UserName = "Cliente1",
                Password = "12345678",
                Nombre = "Cliente1",
                Apellido = "Cliente1",
                Email = "cliente1@ort.edu.ar"
            };

            _carritoContext.Clientes.Add(cliente);
            _carritoContext.SaveChanges();

            Carrito carrito = new Carrito()
            {
                ClienteId = cliente.Id
            };

            _carritoContext.Carritos.Add(carrito);
            _carritoContext.SaveChanges();

            Categoria categoria = new Categoria()
            {
                Nombre = "Categoria1"
            };

            _carritoContext.Categorias.Add(categoria);
            _carritoContext.SaveChanges();

            Empleado empleado = new Empleado()
            {
                DNI = "1234568",
                UserName = "Empleado1",
                Password = "12345678",
                Nombre = "Empleado1",
                Apellido = "Empleado1",
                Email = "empleado1@ort.edu.ar"
            };

            _carritoContext.Empleados.Add(empleado);
            _carritoContext.SaveChanges();

            Producto producto = new Producto()
            {
                Nombre = "Producto1",
                PrecioVigente = 120,
                CategoriaId = categoria.Id
            };

            _carritoContext.Productos.Add(producto);
            _carritoContext.SaveChanges();

            CarritoItem carritoItem = new CarritoItem()
            {
                CarritoId = carrito.Id,
                ProductoId = producto.Id,
                Cantidad = 1
            };

            _carritoContext.CarritoItems.Add(carritoItem);
            _carritoContext.SaveChanges();

            Sucursal sucursal = new Sucursal()
            {
                Direccion = "Calle 1",
            };

            _carritoContext.Sucursales.Add(sucursal);
            _carritoContext.SaveChanges();

            StockItem stockItem = new StockItem()
            {
                ProductoId = producto.Id,
                SucursalId = sucursal.Id,
                Cantidad = 1
            };

            _carritoContext.StockItems.Add(stockItem);
            _carritoContext.SaveChanges();

            Compra compra = new Compra()
            {
                ClienteId = cliente.Id,
                CarritoId = carrito.Id,
                SucursalId = sucursal.Id
            };

            _carritoContext.Compras.Add(compra);
            _carritoContext.SaveChanges();

            return RedirectToAction("Index","Home");
        }
    }
}
