using Carrito_D.Helpers;
using Carrito_D.Models;
using Carrito_D.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Carrito_D.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signinmanager;

        public AccountController(UserManager<Persona> usermanager, SignInManager<Persona> signinmanager)
        {
            this._usermanager = usermanager;
            this._signinmanager = signinmanager;
        }

        public IActionResult Registrar()
        {
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

        public IActionResult IniciarSesion(string returnUrl)
        {
            //ViewBag.Url1 = returnUrl;
            //ViewData["Url2"] = returnUrl;
            TempData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewmodel)
        {
            //var url1 = ViewBag.Url1;
            //var url2 = ViewData["Url2"];
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

        public IActionResult AccesoDenegado(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}

