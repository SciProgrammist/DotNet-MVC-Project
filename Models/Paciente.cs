using System.ComponentModel.DataAnnotations;

namespace Turnos.Models
{
    public class Paciente 
    {
        [Key] //Asi se le indica que esta sera la llave primaria de la entidad
        public int IdPaciente {get; set;}
        public string Nombre {get; set;}
        public string Apellido {get; set;}
        public string Direccion {get; set;}
        public string Telefono {get; set;}
        public string Email {get; set;}

        
    }
}