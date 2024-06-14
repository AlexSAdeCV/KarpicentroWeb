using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using KarpicentroWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using System.IO;

namespace KarpicentroWeb.Controllers
{
    public class BoughtsController : Controller
    {
        public readonly KarpicentroDB _contextDB;
        public int ID { get; set; }
        public int iddir { get; set; }
        public int idord { get; set; }
        public static string Email { get; set; }
        public User usuario { get; set; }

        public BoughtsController(KarpicentroDB karpicentroDB)
        {
            _contextDB = karpicentroDB;
        }

        public void Cookie()
        {
            var miCookie = HttpContext.Request.Cookies["MiCookie"];

            if (miCookie != null)
            {
                List<User> listaUsers = _contextDB.User.ToList();
                foreach (var user in listaUsers)
                {
                    if (miCookie == user.Email)
                    {
                        ViewBag.ID = user.ID;
                        ID = user.ID;
                        Email = user.Email;
                        ViewBag.Nombre = user.Name;
                        ViewBag.Nivel = user.UserType;
                        ViewBag.FotoPerfil = user.Image;
                        usuario = user;
                    }
                }
            }
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            Cookie();

            var car = _contextDB.Cart.Where(u => u.SwitchCartBuy == 1 && u.idUser == ID).ToList();
            var direcciones = _contextDB.Direction.ToList();
            var interdir = _contextDB.UseDir.Where(d => d.idUser == ID);
            var listainter = _contextDB.InterProd.ToList();
            var listaproducto = _contextDB.Product.ToList();

            var viewmodel = new ProductViewModel
            {
                Cart = car,
                Inter = listainter,
                Products = listaproducto
            };

            ViewBag.Direcciones = direcciones;
            ViewBag.InterDir = interdir;

            return View(viewmodel);
        }

        public IActionResult AgregarDireccion()
        {
            Cookie();
            return View();
        }
        [HttpPost]
        public IActionResult AgregarDireccion(Directions directions)
        {
            Cookie();

            _contextDB.Direction.Add(directions);
            _contextDB.SaveChanges();

            var interdir = new UserDirections
            {
                idUser = ID,
                idDirections = _contextDB.Direction.Max(i => i.ID)
            };

            _contextDB.UseDir.Add(interdir);

            _contextDB.SaveChanges();

            return RedirectToAction("Checkout");
        }


        [HttpGet]
        public IActionResult Sale(int dirid)
        {
            Cookie();

            iddir = dirid;

            var user = _contextDB.User.FirstOrDefault(u => u.ID == ID);
            var Udir = _contextDB.UseDir.FirstOrDefault(u => u.idDirections == dirid);

            ViewBag.Direccion = _contextDB.Direction.FirstOrDefault(u => u.ID == dirid);
            ViewBag.Productos = _contextDB.Product.ToList();
            ViewBag.Inter = _contextDB.InterProd.ToList();
            ViewBag.NombreUser = user.Name;

            var carro = _contextDB.Cart.Where(c => c.idUser == ID && c.SwitchCartBuy == 1).ToList();
            var inter = _contextDB.InterProd.ToList();
            var idorder = _contextDB.Cart.Max(c => c.IDOrder) + 1;

            ViewBag.IDOrden = idorder;
            idord = idorder;

            foreach (var item in carro)
            {
                foreach (var i in inter)
                {
                    if (item.idProductInter == i.ID)
                    {
                        var comprada = (Int32)item.Amount;
                        var existencia = (Int32)i.Stock;

                        var resultante = existencia - comprada;

                        if (resultante < 0)
                        {
                            i.Stock = 0;
                        }
                        else
                        {
                            i.Stock = resultante;
                        }

                        _contextDB.InterProd.Update(i);
                        _contextDB.SaveChanges();
                    }
                }
                item.SwitchCartBuy = 2;
                item.Shipping = "Preparando tu producto";
                item.Date = DateTime.Now;
                item.IDOrder = idorder;
                item.iduserdir = Udir.ID;

                _contextDB.Cart.Update(item);
                _contextDB.SaveChanges();
            }

            ViewBag.CarrVen = carro;

            correopdf(Email);

            return View();
        }

        public void correopdf(string e)
        {
            //aawj bzin zxss euma
            byte[] pdfBytes;

            var direccion = _contextDB.Direction.FirstOrDefault(d => d.ID == iddir);

            var carro = _contextDB.Cart.Where(c => c.idUser == ID && c.SwitchCartBuy == 2).ToList();
            var inter = _contextDB.InterProd.ToList();
            var prod = _contextDB.Product.ToList();

            var pedidoDetalles = "";
            foreach (var i in carro)
            {
                if (i.IDOrder == idord)
                {
                    foreach (var j in inter)
                    {
                        if (i.idProductInter == j.ID)
                        {
                            foreach (var k in prod)
                            {
                                if (j.idProducts == k.ID)
                                {
                                    pedidoDetalles += $@"
                                        <p><strong>Nombre del Producto:</strong> {k.Name}</p>
                                        <p><strong>Descripcion:</strong> {k.Description}</p>
                                        <p><strong>Cantidad:</strong> {i.Amount}</p>
                                        <p><strong>Precio:</strong> ${j.SalePrice}</p>
                                        <hr>";
                                }
                            }
                        }
                    }
                }
            }

            decimal total = 0;

            var groupedOrders = _contextDB.Cart.Where(u => u.SwitchCartBuy == 2 && u.idUser == ID && u.IDOrder == idord)
                        .GroupBy(u => u.IDOrder)
                        .Select(g => new { IDOrder = g.Key, TotalPrice = g.Sum(u => u.Price), FirstItem = g.First() });

            foreach (var i in groupedOrders)
            {
                total += i.TotalPrice;
            }

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("centrokarpi@gmail.com", "aawjbzinzxsseuma"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("centrokarpi@gmail.com"),
                Subject = "Confirmación de la compra",
                Body = $@"
                    <html>
                    <body>
                        <h1>¡Gracias por tu compra!</h1>
                        <p>Tu pedido ha sido procesado exitosamente.</p>
                        <div>
                            <p><strong>Detalles del Pedido:</strong></p>
                            <p>Nombre: <strong>{usuario.Name}</strong></p>
                            <p>Correo: <strong>{usuario.Email}</strong></p>
                            <p>Fecha: <strong>{DateTime.Now}</strong></p>
                            <p>ID de Pedido: <strong>{idord}</strong></p>
                            <p>Total: <strong>${total}</strong></p>
                            <hr>
                            <p><strong>Productos:</strong></p>
                            {pedidoDetalles}
                        </div>
                    </body>
                    </html>",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(e);

            smtpClient.Send(mailMessage);
        }
    }
}
