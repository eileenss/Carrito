using Carrito_D.Data;
using Carrito_D.Helpers;
using Carrito_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Carrito_D.Controllers
{
    public class PrecargaController : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly CarritoContext _context;
        public PrecargaController(UserManager<Persona> userManager,RoleManager<IdentityRole<int>> roleManager, CarritoContext context)
        {
            this._userManager = userManager;
            this._roleManager = roleManager; 
            this._context = context; 
        }
        private List<string> roles = new List<string>() { Configs.AdminRolNombre, Configs.ClienteRolNombre, Configs.EmpleadoRolNombre, Configs.UsuarioRolNombre };



        //public PrecargaController(CarritoContext context)
        //{
        //    this._carritoContext = context;
        //}

        public IActionResult Seed() //precarga de datos
        {
            CrearRoles().Wait();
            //CrearAdmin().Wait();
            //CrearEmpleados().Wait();
            //CrearClientes().Wait();








            Cliente cliente = new Cliente()
            {
                Cuil = "20123456780",
                DNI = "1234567",
                PasswordHash = "Password1!",
                Nombre = "Cliente1",
                Apellido = "Cliente1",
                Email = "cliente1@ort.edu.ar",
                UserName = "cliente1@ort.edu.ar"

            };

            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            Carrito carrito = new Carrito()
            {
                ClienteId = cliente.Id
            };

            _context.Carritos.Add(carrito);
            _context.SaveChanges();

            Categoria categoria = new Categoria()
            {
                Nombre = "Categoria1"
            };

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            Empleado empleado = new Empleado()
            {
                DNI = "1234568",
                UserName = "Empleado1",
                PasswordHash = "Password1!",
                Nombre = "Empleado1",
                Apellido = "Empleado1",
                Email = "empleado1@ort.edu.ar"
            };

            _context.Empleados.Add(empleado);
            _context.SaveChanges();

            Producto producto = new Producto()
            {
                Nombre = "Producto1",
                PrecioVigente = 120,
                CategoriaId = categoria.Id
            };

            _context.Productos.Add(producto);
            _context.SaveChanges();

            CarritoItem carritoItem = new CarritoItem()
            {
                CarritoId = carrito.Id,
                ProductoId = producto.Id,
                Cantidad = 1
            };

            _context.CarritoItems.Add(carritoItem);
            _context.SaveChanges();

            Sucursal sucursal = new Sucursal()
            {
                Direccion = "Calle 1",
            };

            _context.Sucursales.Add(sucursal);
            _context.SaveChanges();

            StockItem stockItem = new StockItem()
            {
                ProductoId = producto.Id,
                SucursalId = sucursal.Id,
                Cantidad = 1
            };

            _context.StockItems.Add(stockItem);
            _context.SaveChanges();

            Compra compra = new Compra()
            {
                ClienteId = cliente.Id,
                CarritoId = carrito.Id,
                SucursalId = sucursal.Id
            };

            _context.Compras.Add(compra);
            _context.SaveChanges();

            return RedirectToAction("Index","Home", new {mensaje = "Pre-carga Realizada"});
        }

        //private async Task CrearAdmin()
        //{
        //    throw new NotImplementedException();
        //}

        //private async Task CrearEmpleados()
        //{
        //    throw new NotImplementedException();
        //}
        //private async Task CrearClientes()
        //{
        //    throw new NotImplementedException();
        //}




        private async Task CrearRoles()
        {
            foreach(var roleName in roles) { 
            if(!await _roleManager.RoleExistsAsync(roleName)) {
                    await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
                
                
                
                }
            
            }

        }
    }
}
