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
using Microsoft.AspNetCore.Identity;
using Carrito_D.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Carrito_D.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonasController : Controller
    {
        private readonly CarritoContext _context;
        private readonly UserManager<Persona> _userManager;
        
        public PersonasController(CarritoContext context, UserManager<Persona> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        // GET: Personas
        
        public IActionResult Index()
        {
              return View(_context.Personas.ToList());
        }

        // GET: Personas/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = _context.Personas.FirstOrDefault(p => p.Id == id);

            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(bool EsAdmin,[Bind("Id,DNI,Nombre,Apellido,Telefono,Direccion,Email,FechaAlta")] CrearPersona viewmodel)
        {
            if (ModelState.IsValid)
            {
                Persona persona = new Persona()
                {
                    Email = viewmodel.Email,
                    UserName = viewmodel.Email,
                    Nombre = viewmodel.Nombre,
                    Apellido = viewmodel.Apellido,
                    DNI = viewmodel.DNI
                };

                var resultadoNewPersona = await _userManager.CreateAsync(persona, Configs.Password);

                if (resultadoNewPersona.Succeeded)
                {
                    IdentityResult resultadoAddRole;
                    string rolDefinido;

                    if (EsAdmin)
                    {
                     rolDefinido = Configs.AdminRolNombre;
                    }
                    else
                    {
                     rolDefinido = Configs.UsuarioRolNombre;
                    }

                    resultadoAddRole = await _userManager.AddToRoleAsync(persona, rolDefinido);

                    if (resultadoAddRole.Succeeded)
                    {
                        return RedirectToAction("Index", "Personas");
                    }
                    else
                    {
                        return Content($"No se ha podido agregar el rol {rolDefinido}");
                        //_userManager.DeleteAsync(Persona);
                    }
                }

                foreach (var error in resultadoNewPersona.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            
            }
            return View(viewmodel);
        }

        // GET: Personas/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = _context.Personas.Find(id);

            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Telefono,Direccion")] EditCliente persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            /*Ya tenemos viewmodel de edit
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
                    var personaEnDB = _context.Empleados.Find(persona.Id);

                    if (personaEnDB != null)
                    {
                        personaEnDB.Telefono = persona.Telefono;
                        personaEnDB.Direccion = persona.Direccion;

                        _context.Personas.Update(personaEnDB);
                        _context.SaveChanges();
                    }
                      
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.Id))
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
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FirstOrDefaultAsync(p => p.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Personas == null)
            {
                return Problem("Entity set 'CarritoContext.Personas'  is null.");
            }
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
          return _context.Personas.Any(p => p.Id == id);
        }
    }
}
