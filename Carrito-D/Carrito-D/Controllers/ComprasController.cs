using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_D.Data;
using Carrito_D.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace Carrito_D.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        private readonly CarritoContext _context;

        public ComprasController(CarritoContext context)
        {
            _context = context;
        }

        //GET: Compras
        public IActionResult Index()
        {
            if (User.IsInRole("Cliente"))
            {
                var comprasCliente = _context.Compras
                    .Include(c => c.Carrito)
                    .Include(c => c.Cliente)
                    .Include(c => c.Sucursal)
                    .Where(c => c.ClienteId == ClienteLoginId())
                    .OrderByDescending(c => c.Fecha);

                return View(comprasCliente.ToList());
            }

            DateTime fechaActual = DateTime.Now;
            var compras = _context.Compras
                .Include(c => c.Carrito)
                .Include(c => c.Cliente)
                .Include(c => c.Sucursal)
                .Where(c => c.Fecha.Month == fechaActual.Month)
                .OrderByDescending(c => c.Total);

            return View(compras.ToList());
        }

        // GET: Compras/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = _context.Compras
                .Include(c => c.Carrito)
                .Include(c => c.Cliente)
                .Include(c => c.Sucursal)
                .FirstOrDefault(c => c.Id == id);

            compra.Carrito.CarritoItems = _context.CarritoItems
               .Include(c => c.Producto)
               .Where(c => c.CarritoId == compra.CarritoId)
               .ToList();

            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        [Authorize(Roles = "Cliente")]
        public IActionResult CrearCompra(int idCarrito, int idSucursal)
        {
            int clienteId = ClienteLoginId();
            Cliente cliente = _context.Clientes.Find(clienteId);
            Carrito carrito = _context.Carritos.Find(idCarrito);
            Sucursal sucursal = _context.Sucursales.Find(idSucursal);

            if (carrito == null || sucursal == null)
            {
                return NotFound();
            }

            Compra compra = new Compra()
            {
                ClienteId = clienteId,
                Cliente = cliente,
                CarritoId = idCarrito,
                Carrito = carrito,
                Total = Total(idCarrito),
                SucursalId = idSucursal,
                Sucursal = sucursal
            };

            _context.Compras.Add(compra);
            _context.SaveChanges();

            ModificarEstadoCarrito(carrito, cliente, compra);

            return View(compra);
        }

        private void ModificarEstadoCarrito(Carrito carrito, Cliente cliente, Compra compra)
        {
            carrito.Activo = false;
            carrito.Compra = compra;
            _context.Carritos.Update(carrito);
            _context.SaveChanges();

            Carrito nuevoCarrito = new Carrito() { ClienteId = ClienteLoginId() };
            _context.Carritos.Add(nuevoCarrito);
            _context.SaveChanges();

            cliente.Carritos.Add(nuevoCarrito);
            _context.Clientes.Update(cliente);
            _context.SaveChanges();
        }

        private decimal Total(int idCarrito)
        {
            var carritoItems = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .Where(c => c.CarritoId == idCarrito)
                .ToList();

            decimal total = 0;

            foreach (var item in carritoItems)
            {
                total += item.Cantidad * item.Producto.PrecioVigente;
            }

            return total;
        }

        private int ClienteLoginId()
        {
            int clienteId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return clienteId;
        }

        private bool CompraExists(int id)
        {
            return _context.Compras.Any(c => c.Id == id);
        }

        [Authorize(Roles = "Cliente")]
        public IActionResult ElegirSucursal(int idCarrito)
        {
            TempData["CarritoId"] = idCarrito;

            if (TempData.ContainsKey("SinStock"))
            {
                ViewData["SinStock"] = TempData["SinStock"];
                List<Sucursal> sucursalesConStock = BuscarSucursalConStock(idCarrito);

                if (sucursalesConStock.Count > 0)
                {
                    ViewData["ConStock"] = "Las sucursales con stock son las siguientes: ";
                    ViewData["SucursalesConStock"] = new SelectList(sucursalesConStock, "Id", "Direccion");
                }
            }
            else
            {
                ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion");
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public IActionResult ValidarStock(int idSucursal)
        {
            int idCarrito = (int)TempData["CarritoId"];

            var carritoItems = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .Where(c => c.CarritoId == idCarrito)
                .ToList();

            var stockItems = _context.StockItems
                .Include(s => s.Sucursal)
                .Include(s => s.Producto)
                .Where(s => s.SucursalId == idSucursal)
                .ToList();

            int index = 0;
            bool hayStock = true;

            do
            {
                var carritoItem = carritoItems.ElementAt(index);
                if (!stockItems.Any(s => s.ProductoId == carritoItem.ProductoId && s.Cantidad >= carritoItem.Cantidad))
                {
                    hayStock = false;
                }
                index++;
            } while (carritoItems.Count > index && hayStock);

            if (!hayStock)
            {
                TempData["SinStock"] = "No hay suficiente stock";
                return RedirectToAction(nameof(ElegirSucursal), new { idCarrito });
            }

            DescontarStock(carritoItems, stockItems);
            return RedirectToAction("CrearCompra", new { idCarrito, idSucursal });
        }

        private void DescontarStock(List<CarritoItem> carritoItems, List<StockItem> stockItems)
        {
            foreach (var item in carritoItems)
            {
                StockItem stockItem = stockItems.FirstOrDefault(s => s.ProductoId == item.ProductoId);
                stockItem.Cantidad -= item.Cantidad;

                _context.StockItems.Update(stockItem);
                _context.SaveChanges();
            }
        }

        private List<Sucursal> BuscarSucursalConStock(int idCarrito)
        {
            List<Sucursal> sucsConStock = new List<Sucursal>();

            var carritoItems = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .Where(c => c.CarritoId == idCarrito)
                .ToList();

            var sucursales = _context.Sucursales.ToList();

            var stockItems = _context.StockItems
                .Include(s => s.Sucursal)
                .Include(s => s.Producto)
                .ToList();

            foreach (var sucursal in sucursales)
            {
                var stockSuc = stockItems.FindAll(s => s.SucursalId == sucursal.Id);
                bool hayStock = true;

                foreach (var carritoItem in carritoItems)
                {
                    if (!stockSuc.Any(s => s.ProductoId == carritoItem.ProductoId && s.Cantidad >= carritoItem.Cantidad))
                    {
                        hayStock = false;
                        break;
                    }
                }

                if (hayStock)
                {
                    sucsConStock.Add(sucursal);
                }
            }
            return sucsConStock;
        }
    }
}
