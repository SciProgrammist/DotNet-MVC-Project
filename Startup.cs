using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Turnos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;


namespace Turnos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Middlewear

            //Middlewear para el manejo de sesiones.
            services.AddSession( options =>
            {
                // Si luego de este timepo la sesion no registra ninguna actividad entonces se cerrara.
                options.IdleTimeout = TimeSpan.FromSeconds(300); //300 seg = 5 minutos.
                options.Cookie.HttpOnly = true;
            });

            services.AddControllersWithViews( options =>
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
            );
            
            //La inyeccion de dependencia es agregar servicios a nuestro contenedor, o a la aplicacion
            services.AddDbContext<TurnosContext>(opciones => opciones.UseSqlServer(Configuration.GetConnectionString("TurnosContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Aqui se pueden definiar las paginas para las excepciones.
            if (env.IsDevelopment())
            {
                //Middlewear que devuelve una pagina con la excepcion y la descripcion del error.
                app.UseDeveloperExceptionPage();
            } 
            if (env.IsProduction())
            {
                /** El middlewear, UseExceptionHandler, nos redirije al controlador Home y al 
                 *  action o endpoint Error.
                 */
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
