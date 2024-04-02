using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Turnos.Models;

namespace Turnos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]


        /*
         * Este metodo lo que hace es devolver una vista que dentro de ella se define una nueva clase,
         * la cual se llama ErrorViewModel, la cual define una propiedad RequestId y se le asigna un valor.
         * 
         * Nota importante es que cuando se tiene una variable con un signo de ? es para saber si esta 
         * existe, y cuando se tienen dos ?? es para preguntar si tiene un valor. 
         *
         **/
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
