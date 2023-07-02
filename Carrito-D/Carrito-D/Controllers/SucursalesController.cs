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
using Microsoft.Data.SqlClient;

namespace Carrito_D.Controllers
{
    [Authorize(Roles = "Empleado")]
    public class SucursalesController : Controller
    {
        private readonly CarritoContext _context;

        public SucursalesController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Sucursales
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (TempData.ContainsKey("NoDelete"))
            {
                ViewData["NoDelete"] = TempData["NoDelete"];
            }

            return View(_context.Sucursales.ToList());
        }

        // GET: Sucursales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sucursales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nombre,Direccion,Telefono,Email")] Sucursal sucursal)
        {
            if (ModelState.IsValid)
            {
                _context.Sucursales.Add(sucursal);

                try
                {
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbex)
                {
                    ProcesarDuplicado(dbex);
                }
            }
            return View(sucursal);
        }

        private void ProcesarDuplicado(DbUpdateException dbex)
        {
            SqlException innerException = dbex.InnerException as SqlException;

            if (innerException != null && (innerException.Number == 2637 || innerException.Number == 2601))
            {
                ModelState.AddModelError("Direccion", "Ya existe una sucursal con esa dirección.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, dbex.Message);
            }
        }

        // GET: Sucursales/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Sucursales == null)
            {
                return NotFound();
            }

            var sucursal = _context.Sucursales.Find(id);

            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        // POST: Sucursales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Direccion,Telefono")] Sucursal sucursal)
        {
            if (id != sucursal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sucursalEnDb = _context.Sucursales.Find(sucursal.Id);

                    if (sucursalEnDb != null)
                    {
                        sucursalEnDb.Direccion = sucursal.Direccion;
                        sucursalEnDb.Telefono = sucursal.Telefono;

                        _context.Sucursales.Update(sucursalEnDb);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SucursalExists(sucursal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException dbex)
                {
                    ProcesarDuplicado(dbex);
                }
            }
            return View(sucursal);
        }

        // GET: Sucursales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sucursales == null)
            {
                return NotFound();
            }

            var sucursal = await _context.Sucursales.FirstOrDefaultAsync(s => s.Id == id);

            if (sucursal == null)
            {
                return NotFound();
            }

            return View(sucursal);
        }

        // POST: Sucursales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sucursales == null)
            {
                return Problem("Entity set 'CarritoContext.Sucursales'  is null.");
            }

            var sucursal = await _context.Sucursales.FindAsync(id);

            if (sucursal == null)
            {
                return NotFound();
            }

            if (VerificarStock(id))
            {
                EliminarStockItems(sucursal);
                _context.Sucursales.Remove(sucursal);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            TempData["NoDelete"] = $"No se pudo eliminar la sucursal {sucursal.Nombre} porque todavía tiene stock disponible.";
            return RedirectToAction(nameof(Index));
        }

        private bool SucursalExists(int id)
        {
            return _context.Sucursales.Any(s => s.Id == id);
        }

        private bool VerificarStock(int idSucursal)
        {
            bool sinStock = true;
            var stockItems = _context.StockItems
                .Include(s => s.Sucursal)
                .Where(s => s.SucursalId == idSucursal)
                .ToList();

            foreach (var stockItem in stockItems)
            {
                if (stockItem.Cantidad > 0)
                {
                    sinStock = false;
                    break;
                }
            }

            return sinStock;
        }

        private void EliminarStockItems(Sucursal sucursal)
        {
            var stockItems = _context.StockItems
                    .Include(s => s.Sucursal)
                    .Where(s => s.SucursalId == sucursal.Id)
                    .ToList();

            foreach (var stockItem in stockItems)
            {
                _context.StockItems.Remove(stockItem);
            }
            _context.SaveChanges();
        }
    }
}
