using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_D.Data;
using Carrito_D.Models;

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
            var carritoItemContext = _context.CarritoItem
                .Include(c => c.Carrito)
                .Include(c => c.Producto);

            return View(carritoItemContext.ToList());
        }

        // GET: CarritoItems/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.CarritoItem == null)
            {
                return NotFound();
            }

            var carritoItem = _context.CarritoItem
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .FirstOrDefault(c => c.CarritoId == id);

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
        public IActionResult Create([Bind("ProductoId,CarritoId,Cantidad")] CarritoItem carritoItem)
        {
            if (ModelState.IsValid)
            {
                _context.CarritoItem.Add(carritoItem);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", carritoItem.CarritoId); //"Id","Activo" ?
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", carritoItem.ProductoId);

            return View(carritoItem);
        }

        // GET: CarritoItems/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.CarritoItem == null)
            {
                return NotFound();
            }

            var carritoItem = _context.CarritoItem.Find(id);

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
        public IActionResult Edit(int id, [Bind("ProductoId,CarritoId,Cantidad")] CarritoItem carritoItem)
        {
            if (id != carritoItem.CarritoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.CarritoItem.Update(carritoItem);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoItemExists(carritoItem.CarritoId))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CarritoItem == null)
            {
                return NotFound();
            }

            var carritoItem = await _context.CarritoItem
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(c => c.CarritoId == id);

            if (carritoItem == null)
            {
                return NotFound();
            }

            return View(carritoItem);
        }

        // POST: CarritoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CarritoItem == null)
            {
                return Problem("Entity set 'CarritoContext.CarritoItem'  is null.");
            }
            var carritoItem = await _context.CarritoItem.FindAsync(id);

            if (carritoItem != null)
            {
                _context.CarritoItem.Remove(carritoItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarritoItemExists(int id)
        {
          return _context.CarritoItem.Any(c => c.CarritoId == id);
        }
    }
}
