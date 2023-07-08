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
using Carrito_D.Helpers;
using System.Net;

namespace Carrito_D.Controllers
{
    [Authorize(Roles = "Admin,Empleado")]
    public class EmpleadosController : Controller
    {
        private readonly CarritoContext _context;
        private readonly UserManager<Persona> _userManager;

        public EmpleadosController(CarritoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<IActionResult> Create([Bind("Id,DNI,Nombre,Apellido,Email,Legajo,Telefono,Direccion")] CrearEmpleado viewmodel)
        {
            if (ModelState.IsValid)
            {
                //Empleado empleado = new Empleado()
                //{
                //    DNI = viewmodel.DNI,
                //    Nombre = viewmodel.Nombre,
                //    Apellido = viewmodel.Apellido,
                //    Telefono = viewmodel.Telefono,
                //    Direccion = viewmodel.Direccion,
                //    Email = viewmodel.Email,
                //    UserName = viewmodel.Email,
                //    PasswordHash = Configs.Password
                //};

                Empleado empleado = new Empleado();
                empleado.DNI = viewmodel.DNI;
                empleado.Nombre = viewmodel.Nombre;
                empleado.Apellido = viewmodel.Apellido;
                empleado.Telefono = viewmodel.Telefono;
                empleado.Direccion = viewmodel.Direccion;
                empleado.UserName = viewmodel.DNI;
                var resultadoEmpleado = await _userManager.CreateAsync(empleado, Configs.Password);

                if (resultadoEmpleado.Succeeded)
                {
                    string email = empleado.Legajo + Configs.Email;
                    empleado.Email = email;
                    empleado.UserName = email;
                    var resultadoAddRole = await _userManager.AddToRoleAsync(empleado, Configs.EmpleadoRolNombre);

                    if (resultadoAddRole.Succeeded)
                    {
                        return RedirectToAction("Index", "Empleados");
                    }
                    else
                    {
                        var delete = await _userManager.DeleteAsync(empleado);
                        return Content($"No se ha podido agregar el rol {Configs.EmpleadoRolNombre}");
                    }
                }

                foreach (var error in resultadoEmpleado.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return View(viewmodel);
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
        public IActionResult Edit(int id, [Bind("Id,Telefono,Direccion")] EditCliente empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var empleadoEnDB = _context.Empleados.Find(empleado.Id);

                    if (empleadoEnDB != null)
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

        [HttpGet]
        public IActionResult DniExistente(string dni)
        {
            if (_context.Empleados.Any(e => e.DNI == dni))
            {
                return Json("Ya existe un empleado con ese número de dni.");
            }
            else
            {
                return Json(true);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EmailExistente(string email)
        {
            var emailEmpleado = await _userManager.FindByEmailAsync(email);

            if (emailEmpleado != null)
            {
                return Json("Ya existe un empleado con ese email.");
            }
            else
            {
                return Json(true);
            }
        }
    }
}

