using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carrito_D.Data;
using Carrito_D.Models;
using Carrito_D.ViewModels;

namespace Carrito_D.Controllers
{
    public class ClientesController : Controller
    {
        private readonly CarritoContext _context;

        public ClientesController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public IActionResult Index()
        {
              return View(_context.Clientes.ToList());
        }

        // GET: Clientes/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Cuil,Id,DNI,UserName,PasswordHash,Nombre,Apellido,Telefono,Direccion,Email")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente =  _context.Clientes.Find(id);

            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Cuil,Telefono,Direccion")] EditCliente cliente) //EditCliente es un viewModel
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            /* ya tenemos un viewmodel
            ModelState.Remove("Cuil");
            ModelState.Remove("DNI");
            ModelState.Remove("UserName");
            ModelState.Remove("Password");
            ModelState.Remove("Nombre");
            ModelState.Remove("Apellido");
            ModelState.Remove("Email");*/
              
            if (ModelState.IsValid)
            {
                try
                {
                    var clienteEnDB = _context.Clientes.Find(cliente.Id);

                    if (clienteEnDB != null)
                    {
                        clienteEnDB.Cuil = cliente.Cuil;
                        clienteEnDB.Telefono = cliente.Telefono;
                        clienteEnDB.Direccion = cliente.Direccion;

                        _context.Clientes.Update(clienteEnDB);
                        _context.SaveChanges();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'CarritoContext.Clientes'  is null.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return _context.Clientes.Any(c => c.Id == id);
        }
    }
}
