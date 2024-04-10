using Microsoft.EntityFrameworkCore;
using Turnos.Models;

namespace Turnos.Models
{
    public class TurnosContext : DbContext
    {
        //Aqui se le envia al constructor opciones por default 
        public TurnosContext(DbContextOptions<TurnosContext> opciones)
        : base(opciones)
        {

        }
        //Este nombre del objeto sera el que tendra al final la tabla en el motor SQLServer.
        public DbSet<Especialidad> Especialidad { get; set; }
        //Con este atributo y sus propiedades podemos contar con el objeto cuando lo necesitamos.
        public DbSet<Paciente> Paciente { get; set; }

        /**Esta clase usa el entity framework para crear nuestra entidad en la base de datos. */
        public DbSet<Medico> Medico { get; set; }

        //DbSet para la relacion de las entidades Medico con Especialidad.
        public DbSet<MedicoEspecialidad> MedicoEspecialidad { get; set; }

        public DbSet<Turno> Turno { get; set; }

        //DbSet para el modelo Login
        public DbSet<Login> Login { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
             * Recordar que las firmas que contiene la palabra reservada override es para que se tome 
             * en cuenta el nuevo metodo definido y no el de por default.   
             *         
             */
            modelBuilder.Entity<Especialidad>(entidad =>
            {
                entidad.ToTable("Especialidad"); //Aqui le estamos indicando que nuestra tabla se llamara especialidad

                entidad.HasKey(e => e.IdEspecialidad); //Con esta linea le indicamos que la primarykey de la tabla sera el campo id de la especialidad

                entidad.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false); //Aqui se definieron varias propiedades para el campo descripcion

            }
            );

            modelBuilder.Entity<Paciente>(entidad =>
            {
                entidad.ToTable("Paciente");

                entidad.HasKey(p => p.IdPaciente);

                entidad.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entidad.Property(p => p.Apellido)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entidad.Property(p => p.Direccion)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(true);

                entidad.Property(p => p.Telefono)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

                entidad.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            }
            );

            modelBuilder.Entity<Medico>(entidad =>
            {

                entidad.ToTable("Medico");
                entidad.HasKey(m => m.IdMedico);
                entidad.Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entidad.Property(m => m.Apellido)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entidad.Property(m => m.Direccion)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

                entidad.Property(m => m.Telefono)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

                entidad.Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entidad.Property(m => m.HorarioAtencionDesde)
                .IsRequired()
                .IsUnicode(false);

                entidad.Property(m => m.HorarioAtencionHasta)
                .IsRequired()
                .IsUnicode(false);


            }
            );

            //Normalizando relacion mucho a muchos entra tabla Medico y Especialidad

            modelBuilder.Entity<MedicoEspecialidad>().HasKey(x => new { x.IdMedico, x.IdEspecialidad });

            modelBuilder.Entity<MedicoEspecialidad>().HasOne(x => x.Medico)
            .WithMany(p => p.MedicoEspecialidad)
            .HasForeignKey(p => p.IdMedico);

            /* En la linea 123, se esta definiendo una primary key compuesta por los campos de Id de las tablas a relacionar.
             * En la linea 125, se define una restriccion entre la tabla medico y la tabla MedicoEspecialidad, 
             * con la propiedad hasOne y WithMany se esta estableciendo una relacion de una a muchos, y con la propiedad
             * hasForeignKey se establece el campo que formara parte de la foreing key.
             */

            //En el video no menciona pero al final lo que se hace con esta tabla extra es normalizar la relaicon muchos a muchos
            //que existen entre las entidades Medico y Especialidad, es por eso las lineas de codigo de abajo:

            modelBuilder.Entity<MedicoEspecialidad>().HasOne(x => x.Especialidad)
            .WithMany(p => p.MedicoEspecialidad)
            .HasForeignKey(p => p.IdEspecialidad);


            //Aqui se esta definiendo la entidad Turno en la base de datos.

            modelBuilder.Entity<Turno>(entidad =>
            {
                entidad.ToTable("Turno");
                entidad.HasKey(m => m.IdTurno);

                entidad.Property(m => m.IdPaciente)
                .IsRequired()
                .IsUnicode(false);

                entidad.Property(m => m.IdMedico)
                .IsRequired()
                .IsUnicode(false);

                entidad.Property(m => m.FechaHoraInicio)
                .IsRequired()
                .IsUnicode(false);

                entidad.Property(m => m.FechaHoraFin)
                .IsRequired()
                .IsUnicode(false);

            });

            /* Aqui se esta definiendo las relaciones que tendra la entidad turno en la base de datos.
             * Esta es la restriccion entre la tabla paciente y la tabla turno, donde se especifica que 
             * la relacion sera de uno a muchos, donde un paciente tendra muchos turnos.
             *
             */

             modelBuilder.Entity<Turno>().HasOne(x => x.Paciente)
            .WithMany(p => p.Turno)
            .HasForeignKey(p => p.IdPaciente);

            //Aca se realiza los mismo pero con la tabla medico.

            modelBuilder.Entity<Turno>().HasOne( x => x.Medico)
            .WithMany( p => p.Turno)
            .HasForeignKey(p => p.IdMedico);

            modelBuilder.Entity<Login>(entidad => 
            {
                entidad.ToTable("Login");

                entidad.HasKey( l => l.LoginId);

                entidad.Property(l => l.Usuario)
                .HasMaxLength(50)
                .IsRequired();

                entidad.Property( l=> l.Password)
                .HasMaxLength(100)
                .IsRequired();

            });
            /**Esta clase usa el entity framework para crear nuestra entidad en la base de datos. */

            /* Nota: Ejecuta los siguientes comandos para realizar la Migracion del Modelo Login:
             *
             * dotnet ef migrations add migracionLogin
             * dotnet ef database update
             *
             */


        }


    }
}