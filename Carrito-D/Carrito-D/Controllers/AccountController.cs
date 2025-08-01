﻿using Carrito_D.Data;
using Carrito_D.Helpers;
using Carrito_D.Models;
using Carrito_D.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;


namespace Carrito_D.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signinmanager;
        private readonly CarritoContext _context;

        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signinmanager, CarritoContext context)
        {
            this._usermanager = usermanager;
            this._signinmanager = signinmanager;
            this._context = context;
        }

        public IActionResult Registrar()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AccesoDenegado", new {mensaje = "Ya estás registrado. Cerrá sesión para registrar un nuevo usuario."});
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([Bind("Email,Password,ConfirmedPassword,DNI,Nombre,Apellido")] Registro viewmodel)
        {
            if (ModelState.IsValid)
            {
                Cliente cliente = new Cliente()
                {
                    Email = viewmodel.Email,
                    UserName = viewmodel.Email,
                    Nombre = viewmodel.Nombre,
                    Apellido = viewmodel.Apellido,
                    DNI = viewmodel.DNI
                };

                var resultadoCreate = await _usermanager.CreateAsync(cliente, viewmodel.Password);

                if (resultadoCreate.Succeeded)
                {
                    var resultadoAddRole = await _usermanager.AddToRoleAsync(cliente, Configs.ClienteRolNombre);

                    if (resultadoAddRole.Succeeded)
                    {
                        AgregarCarritoAlCliente(cliente);
                        await _signinmanager.SignInAsync(cliente, isPersistent: false);
                        return RedirectToAction("Edit", "Clientes", new { id = cliente.Id });
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty,$"No se puede agregar el rol de {Configs.ClienteRolNombre}");
                    }          
                }

                foreach(var error in resultadoCreate.Errors)
                {
                    //Recorro los errores del resultadoCreate 
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return View(viewmodel);
        }

        private void AgregarCarritoAlCliente(Cliente cliente)
        {
            Carrito carrito = new Carrito() { ClienteId = cliente.Id };
            _context.Carritos.Add(carrito);
            _context.SaveChanges();

            cliente.Carritos = new List<Carrito> { carrito };
            _context.Clientes.Update(cliente);
            _context.SaveChanges();
        }

        public IActionResult IniciarSesion(string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewmodel)
        {
            string returnUrl = TempData["ReturnUrl"] as string;

            if (ModelState.IsValid)
            {
               var resultado = await _signinmanager.PasswordSignInAsync(viewmodel.Email, viewmodel.Password, viewmodel.Recordarme, false);

                if (resultado.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(String.Empty, "Inicio de sesión inválido.");
            }

            return View(viewmodel);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await _signinmanager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccesoDenegado(string? mensaje)
        {
            ViewBag.ReturnUrl = mensaje;
            return View();
        }

        [HttpGet]
        public IActionResult DniExistente(string dni)
        {
            if (_context.Clientes.Any(c => c.DNI == dni))
            {
                return Json("Ya existe un usuario registrado con ese número de dni.");
            }
            else
            {
                return Json(true);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EmailExistente(string email)
        {
            var emailCliente = await _usermanager.FindByEmailAsync(email);

            if (emailCliente != null)
            {
                return Json("Ya existe un usuario registrado con ese email.");
            }
            else
            {
                return Json(true);
            }
        }
    }
}

