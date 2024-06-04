using CoffeTownNET8.AccesoDatos.Data.Repository.IRepository;
using CoffeTownNET8.Modelos;
using CoffeTownNET8.Modelos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;

namespace CoffeTownNET8.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VentasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public VentasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create(int? id)
        {
            PedidoVM pedidoVM = new PedidoVM()
            {
                Pedido = new CoffeTownNET8.Modelos.Pedido(),
                ListaProductos = _contenedorTrabajo.Producto.GetListaProductos()
            };

            if (id != null)
            {
                pedidoVM.Pedido = _contenedorTrabajo.Pedido.Get(id.GetValueOrDefault());
            }

            return View(pedidoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PedidoVM pedidoVM)
        {
            if (ModelState.IsValid)
            {
                var productoDB = _contenedorTrabajo.Producto.Get(pedidoVM.Pedido.ProductoId);
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("CoffeTown", "milton.reyes1@outlook.es"));
                message.To.Add(new MailboxAddress(pedidoVM.Pedido.Nombre, "cristian057355359@gmail.com"));
                message.Subject = "Factura de su Pedido - CoffeTown";

                // Construir el cuerpo del mensaje con los detalles del pedido
                string emailBody = $@"
                    <h1>Factura de su Pedido</h1>
                    <p>Estimado/a {pedidoVM.Pedido.Nombre},</p>
                    <p>Gracias por su compra. A continuación, encontrará los detalles de su pedido:</p>
                    <table border='1' cellpadding='10'>
                        <tr>
                            <th>Producto</th>
                            <th>Cantidad</th>
                            <th>Precio Unitario</th>
                            <th>Total</th>
                        </tr>
                        <tr>
                            <td>{productoDB.Nombre}</td>
                            <td>{pedidoVM.Pedido.Cantidad}</td>
                            <td>{productoDB.Precio}</td>
                            <td>{pedidoVM.Pedido.MontoTotal}</td>
                        </tr>
                    </table>
                    <p>Fecha del Pedido: {pedidoVM.Pedido.FechaVenta}</p>
                    <p>Total a Pagar: {pedidoVM.Pedido.MontoTotal}</p>
                    <p>Saludos,</p>
                    <p>El equipo de CoffeTown</p>";

                message.Body = new TextPart("html")
                {
                    Text = emailBody
                };

                using (var client = new SmtpClient())
                {
                    // Conectar al servidor SMTP de Outlook
                    client.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                    // Autenticarse con la contraseña específica de la aplicación
                    client.Authenticate("milton.reyes1@outlook.es", "xprsuabmkrtlfyfe");

                    // Enviar el mensaje
                    client.Send(message);
                    // Desconectarse del servidor
                    client.Disconnect(true);
                }

                // Mapear los datos de PedidoVM a Venta
                Venta venta = new Venta
                {
                    FechaVenta = pedidoVM.Pedido.FechaVenta,
                    Nombre = pedidoVM.Pedido.Nombre,
                    ProductoId = pedidoVM.Pedido.ProductoId,
                    Cantidad = pedidoVM.Pedido.Cantidad,
                    MontoTotal = pedidoVM.Pedido.MontoTotal
                };

                _contenedorTrabajo.Venta.Add(venta);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }

            pedidoVM.ListaProductos = _contenedorTrabajo.Producto.GetListaProductos();
            return View(pedidoVM);
        }

        #region LLAMADAS A LA API

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Venta.GetAll(includeProperties: "Producto") });
        }

        #endregion
    }
}
