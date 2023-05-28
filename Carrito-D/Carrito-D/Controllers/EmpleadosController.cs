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
    public class EmpleadosController : Controller
    {
        private readonly CarritoContext _context;

        public EmpleadosController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Empleados
        public IActionResult Index()
        {
              return View(_context.Empleados.ToList());
        }

        // GET: Empleados/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = _context.Empleados.FirstOrDefault(e => e.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Legajo,Id,DNI,UserName,Password,Nombre,Apellido,Telefono,Direccion,Email")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Empleados.Add(empleado);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Telefono,Direccion")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }
            
            ModelState.Remove("DNI");
            ModelState.Remove("UserName");
            ModelState.Remove("Password");
            ModelState.Remove("Nombre");
            ModelState.Remove("Apellido");
            ModelState.Remove("Email");

            if (ModelState.IsValid)
            {
                try
                {
                    var empleadoEnDB = _context.Empleados.Find(empleado.Id);

                    if(empleadoEnDB != null)
                    {
                        empleadoEnDB.Telefono = empleado.Telefono;
                        empleadoEnDB.Direccion = empleado.Direccion;

                        _context.Empleados.Update(empleadoEnDB);
                        _context.SaveChanges();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
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
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.Id == id);

            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empleados == null)
            {
                return Problem("Entity set 'CarritoContext.Empleados'  is null.");
            }
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
          return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
