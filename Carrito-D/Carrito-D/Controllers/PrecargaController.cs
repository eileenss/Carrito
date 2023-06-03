using Carrito_D.Data;
using Carrito_D.Helpers;
using Carrito_D.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Carrito_D.Controllers
{
    public class PrecargaController : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly CarritoContext _context;

        public PrecargaController(UserManager<Persona> userManager, RoleManager<IdentityRole<int>> roleManager, CarritoContext context)
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
            CrearAdmin().Wait();
            CrearEmpleados().Wait();
            CrearClientes().Wait();
         
            Categoria categoria = new Categoria()
            {
                Nombre = "Categoria1"
            };

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            Producto producto = new Producto()
            {
                Nombre = "Producto1",
                PrecioVigente = 120,
                CategoriaId = categoria.Id
            };

            _context.Productos.Add(producto);
            _context.SaveChanges();

            Sucursal sucursal1 = new Sucursal()
            {
                Direccion = "Calle 1",
            };

            _context.Sucursales.Add(sucursal1);
            _context.SaveChanges();

            StockItem stockItem = new StockItem()
            {
                ProductoId = producto.Id,
                SucursalId = sucursal1.Id,
                Cantidad = 1
            };

            _context.StockItems.Add(stockItem);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home", new { mensaje = "Pre-carga Realizada" });
        }

        private async Task CrearRoles()
        {
            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }
        }

        private async Task CrearAdmin()
        {
            var adminEncontrado = _context.Personas.Any(p => p.Nombre == "Admin");

            if (!adminEncontrado)
            {
                Persona admin = new Persona()
                {
                    DNI = "1234568",
                    Nombre = "Admin",
                    Apellido = "Admin",
                    Email = "admin@ort.edu.ar",
                    UserName = "admin@ort.edu.ar"
                };

                var result = await _userManager.CreateAsync(admin, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, Configs.AdminRolNombre);

                    _context.Personas.Add(admin);
                    _context.SaveChanges();
                }
            }
        }

        private async Task CrearEmpleados()
        {
           // var empleadoEncontrado = _context.Empleados.Any(e => e.DNI == "1234567");

           // if (!empleadoEncontrado)
           // {
                Empleado empleado1 = new Empleado()
                {
                    DNI = "1234567",
                    UserName = "empleado1@ort.edu.ar",
                    Nombre = "Empleado1",
                    Apellido = "Empleado1",
                    Email = "empleado1@ort.edu.ar"
                };

                var result = await _userManager.CreateAsync(empleado1, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(empleado1, Configs.EmpleadoRolNombre);
                    _context.Empleados.Add(empleado1);
                    _context.SaveChanges();
                }

           // }

        }

        private async Task CrearClientes()
        {
           // var clienteEncontrado = _context.Clientes.Any(c => c.DNI == "1234569");

           // if (!clienteEncontrado)
           // {
                Cliente cliente1 = new Cliente()
                {
                    DNI = "1234569",
                    UserName = "cliente1@ort.edu.ar",
                    Nombre = "Cliente1",
                    Apellido = "Cliente1",
                    Email = "cliente1@ort.edu.ar"
                };

                var result = await _userManager.CreateAsync(cliente1, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(cliente1, Configs.ClienteRolNombre);
                    _context.Clientes.Add(cliente1);
                    _context.SaveChanges();
                    await CrearCarrito(cliente1.Id);
                }
            //}
        }

        private async Task CrearCarrito(int clienteId)
        {
            Carrito carrito1 = new Carrito() { ClienteId = clienteId };
            _context.Carritos.Add(carrito1);
            _context.SaveChanges();
            await CrearCompra(clienteId, carrito1.Id, 1);
        }

        private async Task CrearCompra(int clienteId, int carritoId, int sucursalId)
        {
            Compra compra1 = new Compra()
            {
                ClienteId = clienteId,
                CarritoId = carritoId,
                SucursalId = sucursalId,
            };

            _context.Compras.Add(compra1);
            _context.SaveChanges();
        }
    }
}
