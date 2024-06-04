using CoffeTownNET8.AccesoDatos.Data.Repository.IRepository;
using CoffeTownNET8.Modelos.ViewModels;
using CoffeTownNET8.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Diagnostics;
using MailKit.Net.Smtp;
using MailKit.Security;

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

        [HttpGet]
        public IActionResult Pedido()
        {
            PedidoVM pedidoVM = new PedidoVM()
            {
                Pedido = new CoffeTownNET8.Modelos.Pedido(),
                ListaProductos = _contenedorTrabajo.Producto.GetListaProductos()
            };

            return View(pedidoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Pedido(PedidoVM pedidoVM)
        {
            var productoFromDb = _contenedorTrabajo.Producto.Get(pedidoVM.Pedido.ProductoId);
            if (ModelState.IsValid)
            {
                pedidoVM.Pedido.MontoTotal = (float)(pedidoVM.Pedido.Cantidad * productoFromDb.Precio);

                pedidoVM.Pedido.FechaVenta = DateTime.Now.ToString();
                _contenedorTrabajo.Pedido.Add(pedidoVM.Pedido);
                _contenedorTrabajo.Save();

                TempData["PedidoCreado"] = "Su pedido estara listo en unos momentos";
                return RedirectToAction(nameof(Index));
            }
            pedidoVM.ListaProductos = _contenedorTrabajo.Producto.GetListaProductos();
            return View(pedidoVM);
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