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
    [Authorize(Roles = "Empleado")]
    public class StockItemsController : Controller
    {
        private readonly CarritoContext _context;

        public StockItemsController(CarritoContext context)
        {
            _context = context;
        }

        // GET: StockItems
        public IActionResult Index(int? sucursalId)
        {
            ViewData["Sucursales"] = _context.Sucursales;

            if (sucursalId != null)
            {
                if (!_context.Sucursales.Any(s => s.Id == sucursalId))
                {
                    return NotFound();
                }

                var filtroSucursal = _context.StockItems
                    .Include(s => s.Producto)
                    .Include(s => s.Sucursal)
                    .Where(s => s.SucursalId == sucursalId);

                return View(filtroSucursal.ToList());
            }

            var stockItems = _context.StockItems
                .Include(s => s.Producto)
                .Include(s => s.Sucursal);

            return View(stockItems.ToList());
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
            if (!StockItemExists(stockItem.ProductoId, stockItem.SucursalId))
            {
                if (ModelState.IsValid)
                {
                    _context.StockItems.Add(stockItem);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            
            ViewData["YaExiste"] = "La sucursal ya tiene stock de ese producto, podés modificarlo desde editar: ";
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
                    var stockItemEnDB = _context.StockItems
                        .FirstOrDefault(s => s.ProductoId == stockItem.ProductoId && s.SucursalId == stockItem.SucursalId);

                    if (stockItemEnDB != null)
                    {
                        stockItemEnDB.Cantidad = stockItem.Cantidad;

                        _context.StockItems.Update(stockItemEnDB);
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

            return View(stockItem);
        }

        private bool StockItemExists(int idProd, int idSuc)
        {
            return _context.StockItems.Any(s => s.ProductoId == idProd && s.SucursalId == idSuc);
        }
    }
}
