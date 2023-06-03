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

        // GET: Compras
        public IActionResult Index()
        {
            var carritoContext = _context.Compras
                .Include(c => c.Carrito)
                .Include(c => c.Cliente)
                .Include(c => c.Sucursal);
            return View( carritoContext.ToList());
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

            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id");
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido");
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion");
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ClienteId,CarritoId,Fecha,SucursalId")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Compras.Add(compra);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", compra.CarritoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", compra.ClienteId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion", compra.SucursalId);
            return View(compra);
        }

        //UNA COMPRA NO DEBERÍA EDITARSE UNA VEZ CREADA
        // GET: Compras/Edit/5
       /* public IActionResult Edit(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = _context.Compras.Find(id);

            if (compra == null)
            {
                return NotFound();
            }
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", compra.CarritoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", compra.ClienteId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion", compra.SucursalId);
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Fecha")] Compra compra)
        {
            if (id != compra.Id)
            {
                return NotFound();
            }

            ModelState.Remove("ClienteId");
            ModelState.Remove("CarritoId");
            ModelState.Remove("SucursalId");


            if (ModelState.IsValid)
            {
                try
                {
                    var compraEnDB = _context.Clientes.Find(compra.Id);

                    if (compraEnDB != null)
                    {
                        compraEnDB.FechaAlta = compra.Fecha;
                        

                        _context.Update(compraEnDB);
                        _context.SaveChanges();
                    }



                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.Id))
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
            ViewData["CarritoId"] = new SelectList(_context.Carritos, "Id", "Id", compra.CarritoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", compra.ClienteId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion", compra.SucursalId);
            return View(compra);
        }*/

        // GET: Compras/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.Compras == null)
    //        {
    //            return NotFound();
    //        }

    //        var compra = await _context.Compras
    //            .Include(c => c.Carrito)
    //            .Include(c => c.Cliente)
    //            .Include(c => c.Sucursal)
    //            .FirstOrDefaultAsync(c => c.Id == id);

    //        if (compra == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(compra);
    //    }

    //    // POST: Compras/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.Compras == null)
    //        {
    //            return Problem("Entity set 'CarritoContext.Compras'  is null.");
    //        }
    //        var compra = await _context.Compras.FindAsync(id);
    //        if (compra != null)
    //        {
    //            _context.Compras.Remove(compra);
    //        }
            
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

        private bool CompraExists(int id)
        {
          return _context.Compras.Any(c => c.Id == id);
        }
    }
}
