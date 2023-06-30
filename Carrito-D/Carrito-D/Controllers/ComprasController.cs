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

        public IActionResult CrearCompra(int idCarrito, int idSucursal)
        {
            int clienteId = ClienteLoginId();
            Cliente cliente = _context.Clientes.Find(clienteId);
            Carrito carrito = _context.Carritos.Find(idCarrito);
            Sucursal sucursal = _context.Sucursales.Find(idSucursal);

            if (carrito == null)
            {
                return NotFound();
            }

            Compra compra = new Compra()
            {
                ClienteId = ClienteLoginId(),
                Cliente = cliente,
                CarritoId = idCarrito,
                Carrito = carrito,
                Total = Total(idCarrito),
                SucursalId = idSucursal,
                Sucursal = sucursal
            };

            _context.Compras.Add(compra);
            _context.SaveChanges();

            carrito.Activo = false;
            carrito.Compra = compra;
            _context.Carritos.Update(carrito);
            _context.SaveChanges();

            Carrito nuevoCarrito = new Carrito() { ClienteId = clienteId };
            _context.Carritos.Add(nuevoCarrito);
            _context.SaveChanges();

            cliente.Carritos.Add(nuevoCarrito);
            _context.Clientes.Update(cliente);
            _context.SaveChanges();

            return View(compra);
        }

        //private decimal Total(int idCarrito)
        //{
        //    var carritoItems = _context.CarritoItems
        //        .Include(c => c.Carrito)
        //        .Include(c => c.Producto)
        //        .Where(c => c.CarritoId == idCarrito)
        //        .ToList();

        //    decimal total = 0;

        //    foreach (var item in carritoItems)
        //    {
        //        total += item.Cantidad * item.Producto.PrecioVigente;
        //    }

        //    return total;
        //}

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
                total += item.Subtotal;
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
    }
}
