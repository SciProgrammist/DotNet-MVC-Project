

using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class LoginController : Controller
    {
        private readonly TurnosContext _context;

        public LoginController(TurnosContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(Login login)
        {
            //Aqui se esta validando que las validaciones en el modelo se cumplan, de caso 
            //contrario de redirecciona a la vista index (el formulario de login).
            if(ModelState.IsValid)
            {
                //EncriptarPassword
                string passwordEncriptado = Encriptar(login.Password);
                
                var loginUsuario = _context.Login.Where( l => l.Usuario == login.Usuario && l.Password == passwordEncriptado)
                .FirstOrDefault();

                if(loginUsuario != null)
                {
                    //Aqui se define una variable de sesion para la aplicacion.
                    HttpContext.Session.SetString("usuario", loginUsuario.Usuario);

                    //Se va al metodo Index, del controlador Home.
                    return RedirectToAction("Index", "Home");

                } else {

                    ViewData["errorLogin"] = "Los datos ingresados son incorrectos.";
                    return View("Index");
                }

            }
            return View("Index");
        }

        // Metodo de encryptacion estandar y generico, que nos permite encryptar con SHA256.
        public string Encriptar(string password)
        {
            //Se crea un espacio using donde se hace uso del objeto sha256Hash.
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    // Aqui se apendizan los valores, y con el argumento x2 se le pide 
                    // que sea en el formato Hexadecimal.
                    stringBuilder.Append(bytes[i].ToString("x2"));
                }
                return stringBuilder.ToString();

            }
        }

        public IActionResult Logout() 
        {
            //Atraves del contexto http se esta limpiando las sesiones del usuario, ejecutando el metodo clear, para limpiar
            // todas las variables de sesion que se tengan definidas.

            HttpContext.Session.Clear();

            //Luego se abre la lista index, que es el login.
            return View("Index");

            
        }
    }
}