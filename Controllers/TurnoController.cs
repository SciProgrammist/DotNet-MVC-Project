using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        // El index: 

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

        // Aqui se esta creando un metodo o un endpoint, que recibe un parametro de tipo integer que se llama idMedico, y devuelve un JsonResult
        public  JsonResult ObtnerTurnos(int idMedico) 
        {
            List<Turno> turnos = new List<Turno>();

            //Aca se le esta llenando al objeto turnos y se le asigna la lista de turnos que tienen el objeto medico.
            turnos = _context.Turno.Where( t => t.IdMedico == idMedico) .ToList();
            

            return Json(turnos); 

            /* Se devuelve un JsonResult porque se implementa el componente full-callendar que necesita que se le envie la informacion en 
             * formato Json.
             */

        }

        // Guardar: 

        [HttpPost]
        public JsonResult GrabarTurno(Turno turno)
        {
            var ok = false ; 
            try
            {
                _context.Turno.Add(turno);
                _context.SaveChanges();
                ok = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(" {0} Excepcion entradas", e);
            }

            //Aqui se le esta asignando al objeto jsonResult, un contenido json con el resultado de la varible ok
            var jsonResult = new { ok = ok};

            return Json(jsonResult); 
        }


        // Eliminar:

        [HttpPost]
        public JsonResult EliminarTurno(int idTurno)
        {
            var ok = false;   
            try 
            {
                var turnoEliminar = _context.Turno.Where( t => t.IdTurno == idTurno).FirstOrDefault();
                if(turnoEliminar != null)
                {
                    _context.Turno.Remove(turnoEliminar);
                    _context.SaveChanges();
                    ok = true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Excepcion encontrada", e);
            }
            
            var jsonResult = new {ok = ok};
            return Json(jsonResult);

        }

        // Metodos para los horarios en el que medico estara trabajando en el turno:

        public string TraerHorarioAtencionDesde(int idMedico)
        {
            var HorarioAtencionDesde = _context.Medico.Where( m => m.IdMedico == idMedico).FirstOrDefault().HorarioAtencionDesde;
            return HorarioAtencionDesde.Hour + ":" + HorarioAtencionDesde.Minute; 
        }

           public string TraerHorarioAtencionHasta(int idMedico)
        {
            var HorarioAtencionHasta = _context.Medico.Where( m => m.IdMedico == idMedico).FirstOrDefault().HorarioAtencionHasta;
            return HorarioAtencionHasta.Hour + ":" + HorarioAtencionHasta.Minute; 
        }
    
    }
}