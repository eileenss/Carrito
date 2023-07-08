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

        //PRE CARGA DE DATOS
        public IActionResult Seed()
        {
            CrearRoles().Wait();
            CrearAdmin().Wait();
            CrearEmpleado().Wait();
            CrearClientes().Wait();
            CrearCategorias().Wait();
            CrearSucursales().Wait();

            return RedirectToAction("Index", "Home", new { mensaje = "Pre-carga realizada" });
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
            var adminExiste = _context.Personas.Any(p => p.Nombre == "Admin");
            if (!adminExiste)
            {
                Persona admin = new Persona()
                {
                    DNI = "admin",
                    Nombre = "Admin",
                    Apellido = "Admin",
                    Email = "admin@ort.edu.ar",
                    UserName = "admin@ort.edu.ar"
                };

                var resultNew = await _userManager.CreateAsync(admin, Configs.Password);

                if (resultNew.Succeeded)
                {
                    var resultRol = await _userManager.AddToRoleAsync(admin, Configs.AdminRolNombre);
                }
            }
        }

        private async Task CrearEmpleado()
        {
            var hayEmpleados = _context.Empleados.Any();
            if (!hayEmpleados)
            {
                Empleado empleado = new Empleado()
                {
                    DNI = "35353535",
                    Nombre = "Jorge",
                    Apellido = "Perez",
                    Email = "empleado@ort.edu.ar",
                    UserName = "empleado@ort.edu.ar"
                };
                var resultNew = await _userManager.CreateAsync(empleado, Configs.Password);
                if (resultNew.Succeeded)
                {
                    var resultRol = await _userManager.AddToRoleAsync(empleado, Configs.EmpleadoRolNombre);
                }
            }
        }

        private async Task CrearClientes()
        {
            var clienteExiste = _context.Clientes.Any();
            if (!clienteExiste)
            {
                Cliente cliente1 = new Cliente()
                {
                    DNI = "1234567",
                    Nombre = "Mariana",
                    Apellido = "Lopez",
                    Email = "cliente1@ort.edu.ar",
                    UserName = "cliente1@ort.edu.ar"
                };
                var resultNew = await _userManager.CreateAsync(cliente1, Configs.Password);
                if (resultNew.Succeeded)
                {
                    var resultRol = await _userManager.AddToRoleAsync(cliente1, Configs.ClienteRolNombre);
                    Carrito carrito = await CrearCarrito(cliente1);
                    if (resultRol.Succeeded && carrito != null)
                    {
                        cliente1.Carritos = new List<Carrito>() { carrito };
                        _context.Clientes.Update(cliente1);
                        _context.SaveChanges();
                    }
                }

                Cliente cliente2 = new Cliente()
                {
                    DNI = "1234568",
                    Nombre = "Oscar",
                    Apellido = "Cruz",
                    Email = "cliente2@ort.edu.ar",
                    UserName = "cliente2@ort.edu.ar"
                };
                var resultNew2 = await _userManager.CreateAsync(cliente2, Configs.Password);
                if (resultNew2.Succeeded)
                {
                    var resultRol = await _userManager.AddToRoleAsync(cliente2, Configs.ClienteRolNombre);
                    Carrito carrito = await CrearCarrito(cliente2);
                    if (resultRol.Succeeded && carrito != null)
                    {
                        cliente2.Carritos = new List<Carrito>() { carrito };
                        _context.Clientes.Update(cliente2);
                        _context.SaveChanges();
                    }
                }
            }
        }

        private async Task<Carrito> CrearCarrito(Cliente cliente)
        {
            if (cliente != null)
            {
                Carrito carrito = new Carrito() { ClienteId = cliente.Id, CarritoItems = new List<CarritoItem>() };
                _context.Carritos.Add(carrito);
                await _context.SaveChangesAsync();
                return carrito;
            }
            return null;
        }

        private async Task CrearCategorias()
        {
            var hayCategorias = _context.Categorias.Any();
            if (!hayCategorias)
            {
                Categoria vinosTintos = new Categoria()
                {
                    Nombre = "Vinos tintos",
                    Descripcion = "Vinos tintos"
                };
                _context.Categorias.Add(vinosTintos);
                await _context.SaveChangesAsync();

                vinosTintos.Productos = new List<Producto>()
                {
                   await CrearProducto(vinosTintos.Id,"Vino Trapiche Malbec","Vino Trapiche Malbec cosecha 2020 750ml",1800, "trapiche-malbec.jpeg"),
                   await CrearProducto(vinosTintos.Id,"Vino Emilia Malbec","Vino Emilia Malbec cosecha 2022 750ml",1300,"emilia-malbec.jpeg"),
                   await CrearProducto(vinosTintos.Id,"Vino Nicasia Blend","Vino Nicasia Blend de Malbecs cosecha 2021 750ml",2500,"nicasia.jpg")
                };
                _context.Categorias.Update(vinosTintos);
                await _context.SaveChangesAsync();

                Categoria whiskies = new Categoria()
                {
                    Nombre = "Whiskies",
                    Descripcion = "Whiskies nacionales e importados"
                };
                _context.Categorias.Add(whiskies);
                await _context.SaveChangesAsync();

                whiskies.Productos = new List<Producto>()
                {
                   await CrearProducto(whiskies.Id,"Whisky Chivas Regal 12 años","Whisky Chivas Regal 12 años 1Lt",17000,"chivas.jpeg"),
                   await CrearProducto(whiskies.Id,"Whisky Johnnie Walker Black","Whisky Johnnie Walker Black 1Lt",18000,"jw-black.jpg"),
                   await CrearProducto(whiskies.Id,"Whisky Johnnie Walker Blue","Whisky Johnnie Walker Blue 700ml",15000,"jw-blue.jpg")
                };
                _context.Categorias.Update(whiskies);
                await _context.SaveChangesAsync();
            }

        }

        private async Task<Producto> CrearProducto(int categoriaId, string nombre, string descripcion, decimal precioVigente, string foto)
        {
            Producto producto = new Producto()
            {
                CategoriaId = categoriaId,
                Nombre = nombre,
                Descripcion = descripcion,
                PrecioVigente = precioVigente,
                Imagen = foto
            };
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        private async Task CrearSucursales()
        {
            var haySucursales = _context.Sucursales.Any();
            if (!haySucursales)
            {
                Sucursal palermo = new Sucursal()
                {
                    Nombre = "Palermo",
                    Direccion = "Av. Santa Fe 1500",
                    Email = "palermo@ort.edu.ar",
                    Telefono = 1147720098
                };
                _context.Sucursales.Add(palermo);
                _context.SaveChanges();

                List<StockItem> stockP = await CrearStockItems(palermo);
                palermo.StockItems = stockP;
                _context.Sucursales.Update(palermo);
                _context.SaveChanges();

                Sucursal flores = new Sucursal()
                {
                    Nombre = "Flores",
                    Direccion = "Av. Rivadavia 4000",
                    Email = "flores@ort.edu.ar",
                    Telefono = 1149920048
                };
                _context.Sucursales.Add(flores);
                _context.SaveChanges();

                List<StockItem> stockF = await CrearStockItems(flores);
                flores.StockItems = stockF;
                _context.Sucursales.Update(flores);
                _context.SaveChanges();

                Sucursal once = new Sucursal()
                {
                    Nombre = "Once",
                    Direccion = "Bartolomé Mitre 544",
                    Email = "once@ort.edu.ar",
                    Telefono = 1148824512
                };
                _context.Sucursales.Add(once);
                _context.SaveChanges();

                List<StockItem> stockO = await CrearStockItems(once);
                once.StockItems = stockO;
                _context.Sucursales.Update(once);
                _context.SaveChanges();
            }
        }

        private async Task<List<StockItem>> CrearStockItems(Sucursal sucursal)
        {
            var productos = _context.Productos.ToList();
            List<StockItem> lista = new List<StockItem>();
            if (_context.Productos != null && sucursal != null)
            {
                foreach (var producto in productos)
                {
                    StockItem st = new StockItem() { ProductoId = producto.Id, SucursalId = sucursal.Id, Cantidad = 5 };
                    _context.StockItems.Add(st);
                    await _context.SaveChangesAsync();
                    lista.Add(st);
                }
            }
            return lista;
        }
    }
}
