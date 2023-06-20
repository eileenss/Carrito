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
    // [Authorize(Roles = "Empleado")]
    public class StockItemsController : Controller
    {
        private readonly CarritoContext _context;

        public StockItemsController(CarritoContext context)
        {
            _context = context;
        }

        // GET: StockItems
        public IActionResult Index()
        {
            var carritoContext = _context.StockItems
                .Include(s => s.Producto)
                .Include(s => s.Sucursal);

            return View(carritoContext.ToList());
        }

        // GET: StockItems/Details/5
        public IActionResult Details(int? idProd, int? idSuc)
        {
            if (idProd == null || idSuc == null)
            {
                return NotFound();
            }

            var stockItem = _context.StockItems
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefault(s => s.ProductoId == idProd && s.SucursalId == idSuc);

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
        public IActionResult Create([Bind("ProductoId,SucursalId,Cantidad")] StockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                _context.StockItems.Add(stockItem);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "Id", "Nombre", stockItem.ProductoId);
            ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion", stockItem.SucursalId);
            return View(stockItem);
        }

        // GET: StockItems/Edit/5

        public IActionResult Edit(int? idProd, int? idSuc)
        {
            if (idProd == null || idSuc == null)
            {
                return NotFound();
            }

            var stockItem = _context.StockItems.Find(idProd, idSuc);

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

        public IActionResult Edit([Bind("ProductoId,SucursalId,Cantidad")] StockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var stockItemEnDB = _context.StockItems.Find(stockItem.ProductoId, stockItem.SucursalId);

                    if (stockItemEnDB != null)
                    {
                        stockItemEnDB.Cantidad = stockItem.Cantidad;

                        _context.StockItems.Update(stockItem);
                        _context.SaveChanges();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockItemExists(stockItem.ProductoId, stockItem.SucursalId))
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

        /* NO PUEDE ELIMINARSE, SOLO SI SE ELIMINA LA SUCURSAL DE LA QUE DEPENDE
        // GET: StockItems/Delete/5
        public async Task<IActionResult> Delete(int? idProd, int? idSuc)
        {
            if (idProd == null || idSuc == null)
            {
                return NotFound();
            }

            var stockItem = await _context.StockItems
                .Include(s => s.Producto)
                .Include(s => s.Sucursal)
                .FirstOrDefaultAsync(s => s.ProductoId == idProd && s.SucursalId == idSuc);
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
            if (_context.StockItems == null)
            {
                return Problem("Entity set 'CarritoContext.StockItems'  is null.");
            }
            var stockItem = await _context.StockItems.FindAsync(id);
            if (stockItem != null)
            {
                _context.StockItems.Remove(stockItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool StockItemExists(int idProd, int idSuc)
        {
            return _context.StockItems.Any(s => s.ProductoId == idProd && s.SucursalId == idSuc);
        }

        public IActionResult ElegirSucursal(int idCarrito)
        {
            TempData["CarritoId"] = idCarrito;
            //ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion");

            if (TempData.ContainsKey("SinStock"))
            {
                ViewData["SinStock"] = TempData["SinStock"];
                List<Sucursal> sucursalesConStock = (List<Sucursal>)TempData["SucursalesConStock"];
                if (sucursalesConStock.Count > 0) { 
                    ViewData["ConStock"] = "Las sucursales con stock son las siguientes: ";
                    ViewData["SucursalId"] = new SelectList(sucursalesConStock , "Id" , "Direccion");
                }
            }
            else
            {
                ViewData["SucursalId"] = new SelectList(_context.Sucursales, "Id", "Direccion");
            }

            return View();
        }

        [HttpPost]
        public IActionResult ValidarStock(int idSucursal)
        {
            int idCarrito = (int)TempData["CarritoId"];
            var carritoItems = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .Where(c => c.CarritoId == idCarrito)
                .ToList();

            var stockItems = _context.StockItems
                .Include(s => s.Sucursal)
                .Include(s => s.Producto)
                .Where(s => s.SucursalId == idSucursal)
                .ToList();

            int index = 0;
            bool hayStock = true;

            do
            {
                var carritoItem = carritoItems.ElementAt(index);

                if (!stockItems.Any(s => s.Cantidad >= carritoItem.Cantidad))
                {
                    hayStock = false;
                }

                index++;

            } while (carritoItems.Count >= index && hayStock);

            if (!hayStock)
            {
                TempData["SinStock"] = "No hay suficiente stock";
                List<Sucursal> sucursalesConStock = BuscarSucursalConStock(idCarrito);
                TempData["SucursalesConStock"] = sucursalesConStock;
                return RedirectToAction(nameof(ElegirSucursal));
            }

            return RedirectToAction(nameof(Index));
        }
        public List<Sucursal> BuscarSucursalConStock(int idCarrito)
        {
            List<Sucursal> sucsConStock = new List<Sucursal>(); 

            var carritoItems = _context.CarritoItems
                .Include(c => c.Carrito)
                .Include(c => c.Producto)
                .Where(c => c.CarritoId == idCarrito)
                .ToList();

            var sucursales = _context.Sucursales.ToList();

            var stockItems = _context.StockItems
                .Include(s => s.Sucursal)
                .Include(s => s.Producto)
                .ToList();

            foreach ( var sucursal in sucursales ) {

                

                var stockSuc = stockItems.FindAll(s => s.SucursalId == sucursal.Id);
                bool hayStock = true;

                foreach (var carritoItem in carritoItems)
                {

                    if(!stockSuc.Any(s => s.ProductoId == carritoItem.ProductoId && s.Cantidad >= carritoItem.Cantidad))
                    {
                        hayStock = false;
                        break;
                    }
                    
                }

                if (hayStock)
                {
                    sucsConStock.Add(sucursal);

                }
                
           
            }

            return sucsConStock;
        }



    }



}
