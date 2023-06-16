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
        [Authorize(Roles ="Cliente")]
        public IActionResult MiCarrito()
        {
            int clienteId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var carrito = _context.Carritos.FirstOrDefault(c => c.ClienteId == clienteId && c.Activo == true);

            var carritoItemContext = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .Where(c => c.CarritoId == carrito.Id);

            return View(carritoItemContext.ToList());
        }

        public IActionResult SumarCantidad(int? idCar, int? idProd)
        {
            if (idCar == null || idProd == null)
            {
                return NotFound();
            }

            var carritoItem = _context.CarritoItems.Find(idCar, idProd);

            if (carritoItem == null)
            {
                return NotFound();
            }

            carritoItem.Cantidad = carritoItem.Cantidad++;
            _context.CarritoItems.Update(carritoItem);
            _context.SaveChangesAsync();

            //CalcularSubtotal(carritoItem);

            return RedirectToAction("MiCarrito", "CarritoItems");
        }

        //public IActionResult RestarCantidad()
        //{
        //    cantidad = cantida - 1
        //        CalcularSubtotal(cantidad)
        //}

        private decimal CalcularSubtotal(decimal valorUnitario , int cantidad)
        {
                decimal subtotal = valorUnitario * cantidad;
                return subtotal;
        }
        
        

        // GET: CarritoItems/Edit/5
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

            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", carritoItem.CarritoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", carritoItem.ProductoId);

            return View(carritoItem);
        }

        // POST: CarritoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

                    if(carritoItemEnDB != null)
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

            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", carritoItem.CarritoId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", carritoItem.ProductoId);
            
            return View(carritoItem);
        }

        // GET: CarritoItems/Delete/5
        public async Task<IActionResult> Delete(int? idCar, int? idProd)
        {
            if (idCar == null || idProd == null)
            {
                return NotFound();
            }

            var carritoItem = await _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(c => c.CarritoId == idCar && c.ProductoId == idProd);

            if (carritoItem == null)
            {
                return NotFound();
            }

            return View(carritoItem);
        }

        // POST: CarritoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idCar, int idProd)
        {
            if (_context.CarritoItems == null)
            {
                return Problem("Entity set 'CarritoContext.CarritoItems'  is null.");
            }
            var carritoItem = await _context.CarritoItems.FindAsync(idCar, idProd);

            if (carritoItem != null)
            {
                _context.CarritoItems.Remove(carritoItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MiCarrito));
        }

        private bool CarritoItemExists(int idCar, int idProd)
        {
            return _context.CarritoItems.Any(c => c.CarritoId == idCar && c.ProductoId == idProd);
        }
        //METODO AGREGAR CARRITO
        [Authorize(Roles = "Cliente")]
        public IActionResult AgregarAlCarrito(int idProducto)
        {
            var producto = _context.Productos.Find(idProducto);

            if (producto == null)
            {
                return NotFound();
            }
            var clienteId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var carrito = _context.Carritos.FirstOrDefault(c => c.ClienteId == clienteId && c.Activo == true);

            CarritoItem carritoItem = new CarritoItem() { CarritoId = carrito.Id, ProductoId = idProducto, Producto = producto, Carrito = carrito, ValorUnitario = producto.PrecioVigente, Cantidad = 1, Subtotal = CalcularSubtotal(producto.PrecioVigente , 1) };
            _context.CarritoItems.Add(carritoItem);
            _context.SaveChanges();
            carrito.CarritoItems.Add(carritoItem);
            _context.Carritos.Update(carrito);
            _context.SaveChanges();

            return RedirectToAction("Details", "Productos", "idProducto");
        }



    }




}
