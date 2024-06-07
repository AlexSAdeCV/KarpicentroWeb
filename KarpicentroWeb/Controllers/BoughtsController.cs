using KarpicentroWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarpicentroWeb.Controllers
{
    public class BoughtsController : Controller
    {
        public readonly KarpicentroDB _contextDB;
        public int ID { get; set; }

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
                        ViewBag.Nombre = user.Name;
                        ViewBag.Nivel = user.UserType;
                        ViewBag.FotoPerfil = user.Image;
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

            var interdir = new UserDirections
            {
                idUser = ID,
                idDirections = _contextDB.Direction.Max(i => i.ID) + 1
            };

            _contextDB.UseDir.Add(interdir);

            _contextDB.SaveChanges();

            return RedirectToAction("Checkout");
        }


        [HttpGet]
        public IActionResult Sale(int dirid)
        {
            Cookie();
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

            return View();
        }
    }
}
