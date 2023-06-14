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
    public class CarritosController : Controller
    {
        private readonly CarritoContext _context;

        public CarritosController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Carritos
        public IActionResult Index()
        {
            int clienteId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!User.IsInRole("Cliente"))
            {
                return RedirectToAction("AccesoDenegado", "Account");
            }

            //var carritoContext = _context.Carritos.Include(c => c.Cliente);
            var carrito = _context.Carritos.Where(c => c.ClienteId == clienteId && c.Activo == true);
            

            return View(carrito);
        }

        /*// GET: Carritos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito =  _context.Carritos
                .Include(c => c.Cliente)
                .FirstOrDefault(c => c.Id == id);

            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }*/

        // GET: Carritos/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido");
            return View();
        }

        // POST: Carritos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Activo,ClienteId")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {
                _context.Carritos.Add(carrito);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }

        // GET: Carritos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito =  _context.Carritos.Find(id);
            if (carrito == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }

        // POST: Carritos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Activo")] Carrito carrito)
        {
            if (id != carrito.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var carritoEnDB = _context.Carritos.Find(carrito.Id);

                    if (carritoEnDB != null)
                    {
                        carritoEnDB.Activo = carrito.Activo;
                        
                        _context.Carritos.Update(carritoEnDB);
                        _context.SaveChanges();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoExists(carrito.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }

        /* NO PUEDE ELIMINARSE
        // GET: Carritos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        // POST: Carritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carritos == null)
            {
                return Problem("Entity set 'CarritoContext.Carritos'  is null.");
            }
            var carrito = await _context.Carritos.FindAsync(id);

            if (carrito != null)
            {
                _context.Carritos.Remove(carrito);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool CarritoExists(int id)
        {
          return _context.Carritos.Any(c => c.Id == id);
        }
    }
}
