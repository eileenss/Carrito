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
                    await _signinmanager.SignInAsync(cliente, isPersistent: false);
                    return RedirectToAction("Edit","Clientes",new { id = cliente.Id });
                }

                foreach(var error in resultadoCreate.Errors)
                {
                    //Recorro los errores del resultadoCreate 
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return View(viewmodel);
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewmodel)
        {
            if (ModelState.IsValid)
            {
               var resultado = await _signinmanager.PasswordSignInAsync(viewmodel.Email, viewmodel.Password, viewmodel.Recordarme, false);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(String.Empty, "Inicio de sesión inválido.");
            }

            return View(viewmodel);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await _signinmanager.SignOutAsync();
            return RedirectToAction("Index", "Homre");
        }
    }
}

