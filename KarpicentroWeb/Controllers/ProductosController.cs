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
        public static int IDProdInter { get; set; }
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

        public IActionResult Products()
        {
            List<Product> listaProductos = _contextDB.Product.ToList();
            List<ProductInter> listainter = _contextDB.InterProd.ToList();

            var viewmodel = new ProductViewModel
            {
                Products = listaProductos,
                Inter = listainter
            };

            Cookie();
            return View(viewmodel);
            return View();
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
            Cookie();

            cartBuys.idUser = ID;
            cartBuys.SwitchCartBuy = 1;
            cartBuys.Shipping = "";

            int c = 0;

            List<CartBuys> listacarr = _contextDB.Cart.ToList();

            foreach (var i in listacarr)
            {
                if (i.idUser == ID)
                {
                    if (i.idProductInter == cartBuys.idProductInter)
                    {
                        cartBuys.Amount += i.Amount;
                        _contextDB.Cart.Remove(i);
                        _contextDB.SaveChanges();
                    }
                    c++;
                }
            }
            if (listacarr.Count == c)
            {
                cartBuys.Price = cartBuys.Price * cartBuys.Amount;
                cartBuys.iduserdir = ID;
                _contextDB.Cart.Add(cartBuys);
                _contextDB.SaveChanges();
            }
            else
            {
                cartBuys.Price = cartBuys.Price * cartBuys.Amount;
                cartBuys.iduserdir = ID;
                _contextDB.Cart.Add(cartBuys);
                _contextDB.SaveChanges();
            }

            return RedirectToAction(nameof(Cart));
        }

        [HttpGet]
        public IActionResult Cart()
        {
            Cookie();
            var listacarrito = _contextDB.Cart.Where(u => u.SwitchCartBuy == 1 && u.idUser == ID).ToList();
            var listainter = _contextDB.InterProd.ToList();
            var listaproducto = _contextDB.Product.ToList();
            ViewBag.ID = ID;

            var viewmodel = new ProductViewModel
            {
                Cart = listacarrito,
                Inter = listainter,
                Products = listaproducto
            };

            return View(viewmodel);
        }

        [HttpGet]
        public IActionResult DeleteCart(int idinter)
        {
            var cartprod = _contextDB.Cart.FirstOrDefault(p => p.idProductInter == idinter);
            _contextDB.Cart.Remove(cartprod);
            _contextDB.SaveChanges();
            return RedirectToAction(nameof(Cart));
        }
    }
}
