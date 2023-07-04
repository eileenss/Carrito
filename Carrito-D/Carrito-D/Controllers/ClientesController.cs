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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

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
        [Authorize(Roles = "Admin,Empleado")]
        public IActionResult Index()
        {
            return View(_context.Clientes.ToList());
        }

        // GET: Clientes/Details/5
        public IActionResult Details(int? id)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }

            if (id == null)
            {
                if (!User.IsInRole("Cliente"))
                {
                    return RedirectToAction("AccesoDenegado", "Account");
                }
                var cliente = _context.Clientes.FirstOrDefault(c => c.Id == ClienteLoginId());

                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            else
            {
                var cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);

                if (cliente == null)
                {
                    return NotFound();
                }

                if (User.IsInRole("Cliente") && ClienteLoginId() != id)
                {
                    return RedirectToAction("AccesoDenegado", "Account");
                }

                return View(cliente);
            }
        }

        // GET: Clientes/Edit/5
        [Authorize(Roles = "Cliente")]
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Cliente") && ClienteLoginId() != id)
            {
                return RedirectToAction("AccesoDenegado", "Account");
            }

            var cliente = _context.Clientes.Find(id);

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
        [Authorize("Cliente")]
        public IActionResult Edit(int id, [Bind("Id,Cuil,Telefono,Direccion")] EditCliente cliente) 
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

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

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(c => c.Id == id);
        }

        private int ClienteLoginId()
        {
            int clienteId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return clienteId;
        }
    }
}
