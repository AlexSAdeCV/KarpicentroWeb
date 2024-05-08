using KarpicentroWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace KarpicentroWeb.Controllers
{
    public class ProductosController : Controller
    {
        public readonly KarpicentroDB _contextDB;

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
            ViewBag.ID = valor;
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
    }
}
