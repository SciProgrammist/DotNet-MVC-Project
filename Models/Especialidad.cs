//using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{
    public class Especialidad
    {
        [Key]
        public int IdEspecialidad { get; set; }
        public string Descripcion { get; set; }

        //Para agragar una relacion con la tabla medico.
        public List<MedicoEspecialidad> MedicoEspecialidad { get; set; }
    }
}