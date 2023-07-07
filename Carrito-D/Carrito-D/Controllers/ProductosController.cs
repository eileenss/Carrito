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
using Carrito_D.ViewModels;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Carrito_D.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Carrito_D.Controllers
{
    public class ProductosController : Controller
    {
        private readonly CarritoContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductosController(CarritoContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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
        public IActionResult Create([Bind("Id,Nombre,Descripcion,Imagen,PrecioVigente,Activo,CategoriaId")] CrearProducto modelo)
        {
            if (ModelState.IsValid)
            {
                //var foto = AgregarFoto()
                Producto producto = new Producto()
                {
                    Nombre = modelo.Nombre,
                    Descripcion = modelo.Descripcion,
                    PrecioVigente = modelo.PrecioVigente,
                    Activo = modelo.Activo,
                    CategoriaId = modelo.CategoriaId
                };
                _context.Productos.Add(producto);

                try
                {
                    _context.SaveChanges();
                    if (!AgregarFoto(producto, modelo.Imagen))
                    {
                        ModelState.AddModelError(string.Empty, "No se pudo cargar la imagen.");
                        return RedirectToAction("Edit", new { id = producto.Id });
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbex)
                {
                    ProcesarDuplicado(dbex);
                }
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", modelo.CategoriaId);

            return View(modelo);
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
        public IActionResult Edit(int id, [Bind("Id,Nombre,Descripcion,PrecioVigente,Activo")] Producto producto)
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

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(p => p.Id == id);
        }

        private bool AgregarFoto(Producto producto, IFormFile imagen)
        {
            bool cargaOk = false;
            string rootPath = _hostingEnvironment.WebRootPath;
            string fotoPath = "img\\fotos";
            string nombreProducto = producto.Nombre;


            if (imagen != null && producto != null)
            {
                string nombreFoto = null;

                if (!string.IsNullOrEmpty(rootPath) && !string.IsNullOrEmpty(fotoPath) && imagen != null)
                {
                    try
                    {
                        string carpetaDestino = Path.Combine(rootPath, fotoPath);
                        nombreFoto = Guid.NewGuid().ToString() + "_" + imagen.FileName;
                        string ruta = Path.Combine(carpetaDestino, nombreFoto);
                        imagen.CopyTo(new FileStream(ruta, FileMode.Create));
                        producto.Imagen = nombreFoto;

                        if (!string.IsNullOrEmpty(producto.Imagen))
                        {
                            _context.Productos.Update(producto);
                            _context.SaveChanges();
                            cargaOk = true;
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "Error en la carga.");
                    }
                }
            }
            return cargaOk;
        }

        
        
        public IActionResult EliminarFoto(int? idProducto)
        {
            if (idProducto == null || idProducto == 0)
            {
                return NotFound();
            }

            EliminarFotoConfirmed(idProducto);

            return RedirectToAction("Edit", new { id = idProducto });
        }

        


        [HttpPost]
        private void EliminarFotoConfirmed(int? idProducto) {

           

            var producto = _context.Productos.Find(idProducto);
            if (producto != null)
            {
                if (producto.Imagen != null)
                {
                    producto.Imagen = Configs.FotoDefault;
                    _context.Productos.Update(producto);
                    _context.SaveChanges();

                }

            }
           
        }

       
    }
}
