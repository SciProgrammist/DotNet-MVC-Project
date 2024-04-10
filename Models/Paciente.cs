using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{
    public class Paciente 
    {
        [Key] //Asi se le indica que esta sera la llave primaria de la entidad
        public int IdPaciente { get; set; }
        [Required (ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }
        [Required (ErrorMessage = "Debe ingresar un apellido")]
        public string Apellido { get; set; }
        [Required (ErrorMessage = "Debe ingresar un dirección")]
        public string Direccion { get; set; }
        [Required (ErrorMessage = "Debe ingresar un teléfono")]
        public string Telefono { get; set; }
        [Required (ErrorMessage = "Debe ingresar un email")]
        [EmailAddress (ErrorMessage = "No es una dirección de email válida")]
        public string Email { get; set; }

        /*
         * Aqui se esta definiendo una propiedad, la cual tiene el modelo turno y su tipo dato es list.
         * esto es para que entityframework pueda realizar la restriccion entre la tabla paciente y la tabla turno
         * lo que seria igual a que un paciente este relacionado con multiples turnos.
         *
        */
    
        public List<Turno> Turno { get; set; }

        
    }
}