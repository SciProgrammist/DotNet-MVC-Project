using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{
    public class Especialidad
    {
        [Key]
        public int IdEspecialidad { get; set; }

        /* Esta DataAnnotation nos sirve para poder mostrar el nombre real al momento de mandar a llamar 
         * el atributo en un formulario en este caso deberia mostrarse con asento el nombre "descripcion"
         * en el formulario.
         */
        [StringLength(200, ErrorMessage = "El campo descripcion debe tener como máximo 5 caracteres.")]
        [Required (ErrorMessage = "Debe ingresar una descripción")]
        [Display (Name = "Descripción", Prompt = "Ingrese una descripción")]
        public string Descripcion { get; set; }

        //Para agragar una relacion con la tabla medico.
        public List<MedicoEspecialidad> MedicoEspecialidad { get; set; }
    }
}