using Microsoft.AspNetCore.Mvc;
using Turnos.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Turnos.Controllers
{
    public class EspecialidadController : Controller
    {
        private readonly TurnosContext _context;

        public EspecialidadController(TurnosContext context)
        {
            _context = context;
        }

        //Este metodo es el que se llama en el protocolo http para retornar vistas
        public  async Task<IActionResult> Index() { 
         /*Con las afirmas que tienen async y Task<> se conviernte las funciones en hilos de procesos 
          *lo cual permite varios llamados a la funcion, y no espera que una llamada se complete. 
          */

            return View(await _context.Especialidad.ToListAsync()); //Se debe tomar en cuenta que la vista se tiene que llamar de la misma manera que el controller.
        }

        public async Task<IActionResult> Edit(int? id) { //El signo de pregunta permite valores nulos, en el caso se llame el metodo
            
            if ( id == null) return NotFound(); //Se devuelve un 404. 

            //mandamos a buscar el usuario con un id especifico a la su entidad correspondiente.
            var especialidad = await _context.Especialidad.FindAsync(id);
            
            if(especialidad == null) return NotFound();

            //Le pasamos los datos a la vista.
            return View(especialidad);

        }

        [HttpPost] //Esto diferencia el metodo Edit que graba, del edit de vista.
        public async Task<IActionResult> Edit(int id, [Bind("IdEspecialidad,Descripcion")] Especialidad especialidad) {
            
            //Para cuando hayan modificaciones en el parametro en el protocolo http del id, se asegure que sea el correcto.
            if(id != especialidad.IdEspecialidad)
            {
                return NotFound();
            }

            if(ModelState.IsValid) //Si esto se devulve true, significa que el bind se hizo correctamente, de los campos del formulario.
            {
                _context.Update(especialidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); //Le estamos pasando el action al cual tiene que redigir una vez guardado los cambios en la base de datos.
            }
            return View(especialidad); 

        }

        public async Task<IActionResult> Delete(int? id)
        {
            //Validacion para el argumento de la funcion
            if (id == null) 
            {
                return NotFound();
            }

            /*
             * Estamos accediendo al modelo especialidad y estamos ejecutando el metodo firstOrDefault
             * este metodo requiere que le demos una propiedad para realizar la busqueda, si no encuentra
             * nada entonces retornara null, el parametro sera una funcion con esa propiedad.
             *
             */
            
            var especialidad = await _context.Especialidad.FirstOrDefaultAsync(e => e.IdEspecialidad == id);
            if(especialidad == null) return NotFound(); //Validando que si exista, en el caso que otro usuario justo en el momento de busqueda lo haya eliminado.

            return View(especialidad);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var especialidad = await _context.Especialidad.FindAsync(id);
            _context.Especialidad.Remove(especialidad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create() {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([Bind("IdEspecialidad,Descripcion")] Especialidad especialidad) {
            /*
             * Este metodo es el que se ejecuta con el protocolo http.
             * Al comprobar que el objeto modelstate devuelve true, quiere decir que todas las validaciones del formulario 
             * y la conexion entre los campos del formulario con el metodo Bind han funcionado correctamente y se han recibido bien 
             * en este metodo, al continuar se puede seguir con la persistencia en la base de datos de la nueva especialidad.
             */


            if(ModelState.IsValid)
            {
                _context.Add(especialidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();

        }

    }
}