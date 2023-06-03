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
        public IActionResult Index()
        {
            var carritoItemContext = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto);

            return View(carritoItemContext.ToList());
        }

        // GET: CarritoItems/Details/5
        public IActionResult Details(int? idCar, int? idProd)
        {
            if (idCar == null || idProd == null)
            {
                return NotFound();
            }

            var carritoItem = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .FirstOrDefault(c => c.CarritoId == idCar && c.ProductoId == idProd);

            if (carritoItem == null)
            {
                return NotFound();
            }

            return View(carritoItem);
        }

        // GET: CarritoItems/Create
        public IActionResult Create()
        {
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id"); //"Id","Activo" ?
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre");
            return View();
        }

        // POST: CarritoItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CarritoId,ProductoId,Cantidad")] CarritoItem carritoItem)
        {
            if (ModelState.IsValid)
            {
                _context.CarritoItems.Add(carritoItem);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", carritoItem.CarritoId); 
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", carritoItem.ProductoId);

            return View(carritoItem);
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
                return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        private bool CarritoItemExists(int idCar, int idProd)
        {
            return _context.CarritoItems.Any(c => c.CarritoId == idCar && c.ProductoId == idProd);
        }
    }
}
