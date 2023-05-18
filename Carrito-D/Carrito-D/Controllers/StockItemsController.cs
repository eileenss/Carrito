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
    public class StockItemsController : Controller
    {
        private readonly CarritoContext _context;

        public StockItemsController(CarritoContext context)
        {
            _context = context;
        }

        // GET: StockItems
        public async Task<IActionResult> Index()
        {
            var carritoContext = _context.StockItem.Include(s => s.Producto).Include(s => s.Sucursal);
            return View(await carritoContext.ToListAsync());
        }

        // GET: StockItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StockItem == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(m => m.SucursalId == id);
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // GET: StockItems/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre");
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion");
            return View();
        }

        // POST: StockItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,SucursalId,Cantidad")] StockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion", stockItem.SucursalId);
            return View(stockItem);
        }

        // GET: StockItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StockItem == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem.FindAsync(id);
            if (stockItem == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion", stockItem.SucursalId);
            return View(stockItem);
        }

        // POST: StockItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,SucursalId,Cantidad")] StockItem stockItem)
        {
            if (id != stockItem.SucursalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockItemExists(stockItem.SucursalId))
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
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion", stockItem.SucursalId);
            return View(stockItem);
        }

        // GET: StockItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StockItem == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItem
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(m => m.SucursalId == id);
            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }

        // POST: StockItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StockItem == null)
            {
                return Problem("Entity set 'CarritoContext.StockItem'  is null.");
            }
            var stockItem = await _context.StockItem.FindAsync(id);
            if (stockItem != null)
            {
                _context.StockItem.Remove(stockItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockItemExists(int id)
        {
          return _context.StockItem.Any(e => e.SucursalId == id);
        }
    }
}
