using Carrito_D.Data;
using Carrito_D.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Carrito_D
{
    public static class StartUp
    {
        public static WebApplication InicializarApp(String[] args)
        {
            //Crear una nueva instancia de nuestro servidor Web
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder); //Lo configuramos con sus respectivos servicios

            var app = builder.Build(); //Sobre esta app configuraremos luego los middleware
            Configure(app); //Configuramos los middleware

            return app; //Retornamos la app ya inicializada
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {

            //builder.Services.AddDbContext<CarritoContext>(options => options.UseInMemoryDatabase("CarritoDb"));
            builder.Services.AddDbContext<CarritoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CarritoDBCS")));

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>

            {
                opciones.LoginPath = "/Account/IniciarSesion";
                opciones.AccessDeniedPath = "/Account/AccesoDenegado";
                opciones.Cookie.Name = "IdentidadCarritoApp";
            });




            //Identity
            builder.Services.AddIdentity<Persona, IdentityRole<int>>().AddEntityFrameworkStores<CarritoContext>();

            builder.Services.Configure<IdentityOptions>(opciones =>
            {
                opciones.Password.RequiredLength = 8;
            }
            );
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
        }

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contexto = serviceScope.ServiceProvider.GetRequiredService<CarritoContext>();

                contexto.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
