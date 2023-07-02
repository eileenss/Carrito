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
using System.Data;
using System.Security.Claims;
using Carrito_D.ViewModels;
using Carrito_D.Helpers;

namespace Carrito_D.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class CarritoItemsController : Controller
    {
        private readonly CarritoContext _context;

        public CarritoItemsController(CarritoContext context)
        {
            _context = context;
        }

        // GET: CarritoItems
        public IActionResult MiCarrito()
        {
            var carrito = _context.Carritos.FirstOrDefault(c => c.ClienteId == ClienteLoginId() && c.Activo == true);

            var carritoItemContext = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .Where(c => c.CarritoId == carrito.Id)
                .ToList();

            foreach(var carritoItem in carritoItemContext)
            {
                carritoItem.Subtotal = Subtotal(carritoItem);
            }
            return View(carritoItemContext);
        }

        private decimal Subtotal(CarritoItem carritoItem)
        {
            decimal subtotal = carritoItem.Producto.PrecioVigente * carritoItem.Cantidad;
            return subtotal;
        }

        //// GET: CarritoItems/Edit/5
        public IActionResult Edit(int? idCarrito, int? idProducto)
        {
            if (idCarrito == null || idProducto == null)
            {
                return NotFound();
            }

            var carritoItem = BuscarCarritoItem(idCarrito, idProducto);

            if (carritoItem == null)
            {
                return NotFound();
            }
           
            return View(carritoItem);
        }

        //// POST: CarritoItems/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("CarritoId,ProductoId,Cantidad")] CarritoItem carritoItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var carritoItemEnDB = _context.CarritoItems.Find(carritoItem.CarritoId, carritoItem.ProductoId);

                    if (carritoItemEnDB != null)
                    {
                        carritoItemEnDB.Cantidad = carritoItem.Cantidad;
                        _context.CarritoItems.Update(carritoItemEnDB);
                        _context.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoItemExists(carritoItem.CarritoId, carritoItem.ProductoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MiCarrito));
            }

            return View(carritoItem);
        }

        // GET: CarritoItems/Delete/5
        public IActionResult Delete(int? idCarrito, int? idProducto)
        {
            if (idCarrito == null || idProducto == null)
            {
                return NotFound();
            }

            var carritoItem = BuscarCarritoItem(idCarrito, idProducto);

            if (carritoItem == null)
            {
                return NotFound();
            }
            return View(carritoItem);
        }

        // POST: CarritoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idCarrito, int idProducto)
        {
            if (_context.CarritoItems == null)
            {
                return Problem("Entity set 'CarritoContext.CarritoItems'  is null.");
            }
            var carritoItem = await _context.CarritoItems.FindAsync(idCarrito, idProducto);

            if (carritoItem != null)
            {
                Carrito carrito = _context.Carritos.Find(idCarrito);
                carrito.CarritoItems.Remove(carritoItem);
                _context.CarritoItems.Remove(carritoItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(MiCarrito));
        }

        private bool CarritoItemExists(int idCar, int idProd)
        {
            return _context.CarritoItems.Any(c => c.CarritoId == idCar && c.ProductoId == idProd);
        }

        public IActionResult AgregarCarritoItem(int? idProducto)
        {
            if (idProducto == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = _context.Productos.Find(idProducto);
            if (!producto.Activo)
            {
                TempData["Pausado"] = $"No se puede agregar al carrito, el producto {producto.Nombre} está pausado.";
                return RedirectToAction("Index", "Productos");
            }

            TempData["ProductoId"] = idProducto;

            return View();
        }

        [HttpPost]
        public IActionResult AgregarCarritoItem([Bind("Cantidad")] CrearCarritoItem viewmodel)
        {
            int idProducto = (int)TempData["ProductoId"];

            if (ModelState.IsValid)
            {
                var producto = _context.Productos.Find(idProducto);
                var carrito = _context.Carritos.FirstOrDefault(c => c.ClienteId == ClienteLoginId() && c.Activo == true);

                if (producto == null || carrito == null)
                {
                    return NotFound();
                }

                CarritoItem carritoItemExists = BuscarCarritoItem(carrito.Id, idProducto);
               
                if (carritoItemExists == null)
                {
                    CarritoItem carritoItem = new CarritoItem()
                    {
                        Cantidad = viewmodel.Cantidad,
                        CarritoId = carrito.Id,
                        ProductoId = idProducto,
                        Carrito = carrito,
                        Producto = producto
                    };

                    _context.CarritoItems.Add(carritoItem);
                    _context.SaveChanges();

                    carrito.CarritoItems.Add(carritoItem);
                    _context.Carritos.Update(carrito);
                    _context.SaveChanges();
                }
                else
                {
                    carritoItemExists.Cantidad = carritoItemExists.Cantidad + viewmodel.Cantidad;
                    _context.CarritoItems.Update(carritoItemExists);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index", "Productos");
            }

            return View(viewmodel);
        }

        private int ClienteLoginId()
        {
            int clienteId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return clienteId;
        }

        public IActionResult VaciarCarrito(int idCarrito)
        {
            VaciarCarritoConfirmed(idCarrito);
            return RedirectToAction(nameof(MiCarrito));
        }

        [HttpPost]
        private void VaciarCarritoConfirmed(int idCarrito)
        {
            var carrito = _context.Carritos.Find(idCarrito);

            var carritoItemContext = _context.CarritoItems
               .Include(c => c.Carrito)
               .Include(c => c.Producto)
               .Where(c => c.CarritoId == idCarrito);

            foreach(var carritoItem in carritoItemContext.ToList())
            {
                carrito.CarritoItems.Remove(carritoItem);
                _context.CarritoItems.Remove(carritoItem);
                _context.SaveChanges();
            }
        }

        private CarritoItem BuscarCarritoItem(int? idCarrito, int? idProducto)
        {
            CarritoItem carritoItem = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .FirstOrDefault(c => c.CarritoId == idCarrito && c.ProductoId == idProducto);
            return carritoItem;
        }

    }
}
