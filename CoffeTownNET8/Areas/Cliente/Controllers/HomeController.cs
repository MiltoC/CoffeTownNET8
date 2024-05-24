using CoffeTownNET8.AccesoDatos.Data.Repository.IRepository;
using CoffeTownNET8.Modelos.ViewModels;
using CoffeTownNET8.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoffeTownNET8.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        public IActionResult Index()

        {
            HomeVM homeVm = new HomeVM()
            {
                Sliders = _contenedorTrabajo.Slider.GetAll(),
                ListProductos = _contenedorTrabajo.Producto.GetAll()
            };

            ViewBag.IsHome = true;

            return View(homeVm);
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var productoDesdeBd = _contenedorTrabajo.Producto.Get(id);
            return View(productoDesdeBd);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}