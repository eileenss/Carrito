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

namespace Carrito_D.Controllers
{
    public class CarritoItemsController : Controller
    {
        private readonly CarritoContext _context;

        public CarritoItemsController(CarritoContext context)
        {
            _context = context;
        }

        // GET: CarritoItems
        [Authorize(Roles = "Cliente")]
        public IActionResult MiCarrito()
        {
            var carrito = _context.Carritos.FirstOrDefault(c => c.ClienteId == ClienteLoginId() && c.Activo == true);

            var carritoItemContext = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .Where(c => c.CarritoId == carrito.Id);

            foreach(var carritoItem in carritoItemContext.ToList())
            {
                carritoItem.Subtotal = Subtotal(carritoItem.Producto.PrecioVigente, carritoItem.Cantidad);
            }

            return View(carritoItemContext.ToList());
        }

        private decimal Subtotal(decimal precio, int cantidad)
        {
            decimal subtotal = precio * cantidad;

            return subtotal;
        }

        //// GET: CarritoItems/Edit/5
        [Authorize(Roles = "Cliente")]
        public IActionResult Edit(int? idCar, int? idProd)
        {
            //if (id == null || _context.CarritoItems == null) creado por scaff
            if (idCar == null || idProd == null)
            {
                return NotFound();
            }

            var carritoItem = _context.CarritoItems.Find(idCar, idProd);

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
        [Authorize(Roles = "Cliente")]
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
        public async Task<IActionResult> Delete(int? idCarrito, int? idProducto)
        {
            if (idCarrito == null || idProducto == null)
            {
                return NotFound();
            }

            var carritoItem = await _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(c => c.CarritoId == idCarrito && c.ProductoId == idProducto);

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

        [Authorize(Roles = "Cliente")]
        public IActionResult AgregarCarritoItem(int idProducto)
        {
            TempData["ProductoId"] = idProducto;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
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

                CarritoItem carritoItemExists = _context.CarritoItems.FirstOrDefault(c => c.CarritoId == carrito.Id && c.ProductoId == idProducto);
               
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

    }
}
