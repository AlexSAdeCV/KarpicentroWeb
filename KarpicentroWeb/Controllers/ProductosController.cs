using KarpicentroWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;

namespace KarpicentroWeb.Controllers
{
    public class ProductosController : Controller
    {
        public readonly KarpicentroDB _contextDB;
        public static int ID { get; set; }
        public static int IDProdInter {  get; set; }
        public static int IDProd { get; set; }

        public ProductosController(KarpicentroDB karpicentroDB)
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
        public IActionResult ProductList(string valor)
        {
            ViewBag.Tipo = valor;
            List<Product> listaProductos = _contextDB.Product.ToList();
            List<ProductInter> listainter = _contextDB.InterProd.ToList();

            var viewmodel = new ProductViewModel
            {
                Products = listaProductos,
                Inter = listainter
            };

            Cookie();
            return View(viewmodel);
        }

        [HttpGet]
        public IActionResult ProductDetails(int valor)
        {
            ViewBag.IDproducto = valor;
            var producto = _contextDB.Product.First(p => p.ID == valor);
            var stock = _contextDB.InterProd.First(p => p.ID == valor);
            ViewBag.Existencia = stock.Stock;

            List<Product> listaProductos = _contextDB.Product.ToList();
            List<ProductInter> listainter = _contextDB.InterProd.ToList();

            var viewmodel = new ProductViewModel
            {
                Products = listaProductos,
                Inter = listainter
            };

            Cookie();
            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult ProductDetails(int idprod, CartBuys cartBuys, int Existencia, int idProductInter)
        {
            if (Convert.ToInt32(Existencia) >= cartBuys.Amount)
            {
                IDProdInter = idProductInter;
                ViewBag.IDProdInter = IDProdInter;
                IDProd = idprod;
                var IDOrder = _contextDB.Cart.Max(o => o.IDOrder) + 1;
                cartBuys.SwitchCartBuy = 1;
                cartBuys.Shipping = ""; 

                int c = 0;

                Cookie();

                var listacarrito = _contextDB.Cart.Where(u => u.IDOrder == IDOrder).ToList();

                foreach (var ca in listacarrito)
                {
                    if (cartBuys.idProductInter == ca.idProductInter)
                    {
                        cartBuys.Amount += ca.Amount;
                        _contextDB.Cart.Remove(ca);
                        _contextDB.SaveChanges();
                        break;
                    }
                    c++;
                }
                if (listacarrito.Count == c)
                {
                    cartBuys.IDOrder = IDOrder;
                    cartBuys.Price *= cartBuys.Amount;
                    cartBuys.idProductInter = idProductInter;
                    cartBuys.idUser = ID;
                    _contextDB.Cart.Add(cartBuys);
                    _contextDB.SaveChanges();
                }
                else
                {
                    cartBuys.IDOrder = IDOrder;
                    cartBuys.Price *= cartBuys.Amount;
                    cartBuys.idProductInter = idProductInter;
                    cartBuys.idUser = ID;
                    _contextDB.Cart.Add(cartBuys);
                    _contextDB.SaveChanges();
                }
            }
            else
            {
                ViewBag.IDproducto = idprod;
                var producto = _contextDB.Product.First(p => p.ID == idprod);
                var stock = _contextDB.InterProd.First(p => p.ID == idprod);
                ViewBag.Existencia = stock.Stock;

                var listaProductos = _contextDB.Product.ToList();
                var listainter = _contextDB.InterProd.ToList();

                var viewmodel = new ProductViewModel
                {
                    Products = listaProductos,
                    Inter = listainter
                };

                return View(viewmodel);
            }

            Cookie();
            return RedirectToAction(nameof(Cart));
        }

        [HttpGet]
        public IActionResult Cart()
        {
            Cookie();
            var listacarrito = _contextDB.Cart.Where(u => u.SwitchCartBuy == 1 && u.idUser == ID).ToList();
            var listainter = _contextDB.InterProd.ToList();

            ViewBag.Image = listainter.FirstOrDefault(i => i.ID == IDProd).Image;
            ViewBag.Products = _contextDB.Product.FirstOrDefault(i => i.ID == IDProd);
            ViewBag.Products = _contextDB.Product.FirstOrDefault(i => i.ID == IDProd);
            ViewBag.Button = listacarrito.Count;

            Cookie();

            return View(listacarrito);
        }
    }
}
