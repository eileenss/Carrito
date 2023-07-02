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
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Carrito_D.Controllers
{
    public class ProductosController : Controller
    {
        private readonly CarritoContext _context;

        public ProductosController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Productos
        public IActionResult Index(int? categoriaId)
        {
            ViewData["Categorias"] = _context.Categorias;

            if (TempData.ContainsKey("Pausado"))
            {
                ViewData["Pausado"] = TempData["Pausado"];
            }

            if (categoriaId != null)
            {
                if (!_context.Categorias.Any(c => c.Id == categoriaId))
                {
                    return NotFound();
                }

                var productosCategoria = _context.Productos
                       .Include(p => p.Categoria)
                       .Where(p => p.CategoriaId == categoriaId);

                return View(productosCategoria.ToList());
            }

            var productos = _context.Productos.Include(p => p.Categoria);

            return View(productos.ToList());
        }

        // GET: Productos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = _context.Productos.Include(p => p.Categoria).FirstOrDefault(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public IActionResult Create([Bind("Id,Nombre,Descripcion,Imagen,PrecioVigente,Activo,CategoriaId")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);

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

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);

            return View(producto);
        }

        private void ProcesarDuplicado(DbUpdateException dbex)
        {
            SqlException innerException = dbex.InnerException as SqlException;
            if (innerException != null && (innerException.Number == 2637 || innerException.Number == 2601))
            {
                ModelState.AddModelError("Nombre", "Ya existe un producto con ese nombre");
            }
            else
            {
                ModelState.AddModelError(string.Empty, dbex.Message);
            }
        }

        // GET: Productos/Edit/5
        [Authorize(Roles = "Empleado")]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = _context.Productos.Find(id);

            if (producto == null)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);

            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public IActionResult Edit(int id, [Bind("Id,Nombre,Descripcion,PrecioVigente,Imagen,Activo")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            //ModelState.Remove("CategoriaId");

            if (ModelState.IsValid)
            {
                try
                {
                    var productoEnDb = _context.Productos.Find(producto.Id);

                    if (productoEnDb != null)
                    {
                        productoEnDb.Nombre = producto.Nombre;
                        productoEnDb.Descripcion = producto.Descripcion;
                        productoEnDb.Imagen = producto.Imagen;
                        productoEnDb.PrecioVigente = producto.PrecioVigente;
                        productoEnDb.Activo = producto.Activo;

                        _context.Productos.Update(productoEnDb);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        /* NO PUEDE ELIMINARSE
        // GET: Productos/Delete/5
      /*  public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
       [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'CarritoContext.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(p => p.Id == id);
        }
    }
}
