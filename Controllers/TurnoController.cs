using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class TurnoController : Controller
    {

        private readonly TurnosContext _context; 

        private IConfiguration _configuration; 

        public TurnoController(TurnosContext context, IConfiguration configuration) 
        {
            _context = context; // Se le asigna el contexto de la base de datos.
            _configuration = configuration; // Configuracion para poder operar con LinQ en nuestra base de datos.

            /* Notes: Language Integrated Query is a Microsoft .NET Framework component that adds 
               native data querying capabilities to .NET languages. */
        }

        public IActionResult Index() 
        {
            /*
             * Aqui se estan definiendo en este objeto la lista de todos los medicos que estan disponibles en el sistema.
             * para mostrarla en un listbox desplegable en la vista turnos.
             *
             */

            ViewData["IdMedico"] = new SelectList((

                // sub-quary en la entidad medico.
                from medico in _context.Medico.ToList() 
                // SelectList(System.Collections.IEnumerable items, aqui tambien se devuelven las propiedades seleccionadas 
                //y se les asigna el objeto NombreCompleto.
                select new { idMedico = medico.IdMedico, NombreCompleto = medico.Nombre + " " + medico.Apellido}), 
                // String dataValueField
                "IdMedico",
                // String dataTextField
                "NombreCompleto"
                );

            ViewData["IdPaciente"] = new SelectList((

                // sub-quary en la entidad medico.
                from paciente in _context.Paciente.ToList() 
                // SelectList(System.Collections.IEnumerable items, aqui tambien se devuelven las propiedades seleccionadas 
                //y se les asigna el objeto NombreCompleto.
                select new { idMedico = paciente.IdPaciente, NombreCompleto = paciente.Nombre + " " + paciente.Apellido}), 
                // String dataValueField
                "IdPaciente",
                // String dataTextField
                "NombreCompleto"
                );


            return View();
        }
        
    }
}