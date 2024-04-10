using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class MedicoController : Controller
    {
        private readonly TurnosContext _context;

        public MedicoController(TurnosContext context)
        {
            _context = context;
        }

        // GET: Medico
        public async Task<IActionResult> Index()
        {
            return View(await _context.Medico.ToListAsync());
        }

        // GET: Medico/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Para mostras la informacion del objeto medico realizamos innerjoin con la palabra reservada .Include
            var medico = await _context.Medico
                .Where( m => m.IdMedico == id).Include( me => me.MedicoEspecialidad)
                .ThenInclude( e => e.Especialidad).FirstOrDefaultAsync();
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // GET: Medico/Create
        public IActionResult Create()
        {
            /*
             * Se creo un nuevo objeto de tipo ViewData al que se le llamo ListaEspecialidades, este objeto de tipo matriz, o diccionario de datos
             * tendra todas las especialidades, que apareceran en el Create.cshtml que manda a llamar esta funcion.
             */
            ViewData["ListaEspecialidades"] = new SelectList(_context.Especialidad, "IdEspecialidad","Descripcion");
            return View();
        }

        // POST: Medico/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMedico,Nombre,Apellido,Direccion,Telefono,Email,HorarioAtencionDesde,HorarioAtencionHasta")] Medico medico, int IdEspecialidad)
        {
            ViewData["ListaEspecialidades"] = new SelectList(_context.Especialidad, "IdEspecialidad","Descripcion",IdEspecialidad);
            if (ModelState.IsValid)
            {
                //En esta primera instancia se tiene la persistencia en la base de datos de la Entidad Medico
                _context.Add(medico);
                await _context.SaveChangesAsync();

                // Aqui se esta haciendo la relacion del medico con sus especialidad y se esta persistiendo la relacion en la base de datos.
                var medicoEspecialidad = new MedicoEspecialidad();
                medicoEspecialidad.IdMedico =  medico.IdMedico;
                medicoEspecialidad.IdEspecialidad = IdEspecialidad;
                _context.Add(medicoEspecialidad);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        // GET: Medico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Para mostrar la edicion del medico con las especialidades, se realizara mediante una consulta linkQ (mediante un innerjoin).
            //var medico = await _context.Medico.FindAsync(id);

            var medico = await _context.Medico.Where( m => m.IdMedico == id)
            .Include(me => me.MedicoEspecialidad).FirstOrDefaultAsync();

            if (medico == null)
            {
                return NotFound();
            }

            SelectList especialidades = new SelectList
            (
                _context.Especialidad, "IdEspecialidad",
                 "Descripcion", 
                 medico.MedicoEspecialidad.Count > 0? medico.MedicoEspecialidad[0].IdEspecialidad : 0
            );

            ViewData["ListaEspecialidades"] = especialidades;


            return View(medico);
        }

        // POST: Medico/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMedico,Nombre,Apellido,Direccion,Telefono,Email,HorarioAtencionDesde,HorarioAtencionHasta")] Medico medico, int IdEspecialidad)
        {
            ViewData["ListaEspecialidades"] = new SelectList(_context.Especialidad, "IdEspecialidad","Descripcion",IdEspecialidad);
            if (id != medico.IdMedico)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    //Esta es la forma para guardar una table sin su relacion todavia...
                    _context.Update(medico);
                    await _context.SaveChangesAsync();

                    //En esta parte se guarda la relacion de la entidad medico con su especialidad.
                    var medicoEspecialidad = await _context.MedicoEspecialidad
                    .FirstOrDefaultAsync( me => me.IdMedico == id);

                    //Debido a que ef no permite actualizar campos que son primary key vamos a hacer que se elimine esta primarykey antes
                    // de escribir sobre ella otro campo.

                    _context.Remove(medicoEspecialidad); // se elimina el registro que se obtuvo en la consulta anterior a la bd.
                    await _context.SaveChangesAsync();
                   
                    medicoEspecialidad.IdEspecialidad = IdEspecialidad;
                    
                    _context.Add(medicoEspecialidad);
                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException) //Esta exception es para el tratamiento de concurrencia de grabacion.
                {
                    if (!MedicoExists(medico.IdMedico)) //Sino existe entonces devuelve un NotFound
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; //Si existe entonces con el throw se sale del try-catch, finalizando la actualizacion a pesar de la concurrencia.
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        // GET: Medico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medico
                .FirstOrDefaultAsync(m => m.IdMedico == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // POST: Medico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicoEspecialidad = await _context.MedicoEspecialidad
            .FirstOrDefaultAsync( me => me.IdMedico == id);

            _context.MedicoEspecialidad.Remove(medicoEspecialidad);
             await _context.SaveChangesAsync();

            var medico = await _context.Medico.FindAsync(id);
            _context.Medico.Remove(medico);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicoExists(int id)
        {
            return _context.Medico.Any(e => e.IdMedico == id);
        }

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
