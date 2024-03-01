using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class PacienteController : Controller
    {
        private readonly TurnosContext _context;

        // Constructor de la Clase.

        public PacienteController(TurnosContext context) {
            _context = context;
        }

        /*
         * Metodo Index devuelve las 
         */

        public async Task<ActionResult> Index() {
            return View(await _context.Paciente.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id) {

            //Validacion para el id.
            if (id == null) return NotFound();

            //Usando un metodo async para especificar mediante una funcion lamba la busqueda que debe realizar.
            var paciente = await _context.Paciente.FirstOrDefaultAsync( p => p.IdPaciente == id);

            //Validar que se haya encontrado correctamente un objeto al momento de satisfacer la condicion. 
            if (paciente == null) return NotFound();

            //Devolver a la vista el objeto encontrado para hacer uso de sus propiedades y metodos.
            return View(paciente);

        }

        //Retorna una vista con el formulario para crear paciente.
        public IActionResult Create() { 
            /* Este metodo no tiene propiedades async porque es un metodo 
               muy sencillo de ejecutar. */
            return View();
        }

        /*
         * [ValidateAntiForgeryToken]
         * Esta propiedad lo que hace es validar nuestro metodo Create hacido ejecutado
         * atraves del formulario y no ha sido ejecutado mediante  la URL del navegador,
         * esto previene ataques fuera de la aplicacion, como de hackers que usan este 
         * protocolo para causar problemas.
         *
         * 
         **/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPaciente,Nombre,Apellido,Direccion,Telefono,Email")] Paciente paciente) {

            if(ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(paciente);

        }

        
        // Metodo Get -Edit
        public async Task<ActionResult> Edit(int? id)
        {
            if( id == null) return NotFound();

            var paciente = await _context.Paciente.FindAsync(id);

            if ( paciente == null) return NotFound();

            return View(paciente);
            
        }

        // Metodo POST -Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("IdPaciente,Nombre,Apellido,Direccion,Telefono,Email")] Paciente paciente) {
            
            if (id != paciente.IdPaciente) return NotFound();

            if(ModelState.IsValid) { //Esta validacion es para comprabar que las validaciones del formulario fueron exitosas.
                _context.Update(paciente);
                await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
            }

            return View(paciente);
        }

        // Metodo GET -Delete

        public async Task<IActionResult> Delete(int? id) 
        {
            if( id == null) return NotFound();

            // El motodo FirstOrDefault nos devuelve el paciente que cumple con el id que deseamos.
            var paciente = await _context.Paciente.FirstOrDefaultAsync( p => p.IdPaciente == id);
            if (paciente == null) return NotFound();

            return View(paciente);

        }

        // Metodo POST -Delete
        [HttpPost, ActionName("Delete")] //Aqui como en el controller se esta usando un nombre diferente, aseguramos con el ActionName se puede seguir manteniendo el nombre en la vista.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {

            if (id == null) return NotFound();

            var paciente = await _context.Paciente.FindAsync(id);

            if (paciente == null) return NotFound();

            _context.Paciente.Remove(paciente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        
    }
}