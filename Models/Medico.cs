using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{
    public class Medico
    {
        [Key]
        public int IdMedico { get; set; } 

        [Required (ErrorMessage="Debe ingresar un nombre")]
        public string Nombre { get; set; }

        [Required (ErrorMessage="Debe ingresar un apellido ")]
        public string Apellido { get; set; }

        [Required (ErrorMessage="Debe ingresar una dirección ")]
        [ Display (Name = "Dirección")]
        public string Direccion { get; set; }

        [Required (ErrorMessage="Debe ingresar un Teléfono ")]
        [ Display (Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required (ErrorMessage="Debe ingresar un  Email")]
        [EmailAddress (ErrorMessage = "No es una direccion de email válida")]
        public string Email{ get; set; }

        [Display (Name = "Horario desde")] 
        [DataType (DataType.Time)]
        [DisplayFormat (DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true )]
        public DateTime HorarioAtencionDesde { get; set; }

        [Display (Name = "Horario hasta")]
        [DataType (DataType.Time)]
        [DisplayFormat (DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]

        public DateTime HorarioAtencionHasta { get; set; }

        /* Para relacionarlo con una especialidad (Entidad Especialidad)
           MedicoEspecialidad una a muchos */
        public List<MedicoEspecialidad> MedicoEspecialidad { get; set; }
        

    }
}