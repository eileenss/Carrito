using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_D.Data;
using Carrito_D.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace Carrito_D.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CarritoContext _context;

        public CategoriasController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public IActionResult Index()
        {
            return View(_context.Categorias.ToList());
        }

        // GET: Categorias/Create
        [Authorize(Roles = "Admin, Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Empleado")]
        public IActionResult Create([Bind("Id,Nombre,Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categorias.Add(categoria);
                try
                {
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException dbex)
                {
                    ProcesarDuplicado(dbex);
                }
            }
            return View(categoria);
        }

        private void ProcesarDuplicado(DbUpdateException dbex)
        {
            SqlException innerException = dbex.InnerException as SqlException;
            if (innerException != null && (innerException.Number == 2637 || innerException.Number == 2601))
            {
                ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre");
            }
            else
            {
                ModelState.AddModelError(string.Empty, dbex.Message);
            }
        }

        // GET: Categorias/Edit/5
        [Authorize(Roles = "Admin, Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Empleado")]
        public IActionResult Edit(int id, [Bind("Id,Nombre,Descripcion")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var categoriaEnDB = _context.Categorias.Find(categoria.Id);

                    if (categoriaEnDB != null)
                    {
                        categoriaEnDB.Nombre = categoria.Nombre;
                        categoriaEnDB.Descripcion = categoria.Descripcion;

                        _context.Categorias.Update(categoriaEnDB);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(c => c.Id == id);
        }
    }
}
