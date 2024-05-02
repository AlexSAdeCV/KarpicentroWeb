using KarpicentroWeb.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index(string valor)
        {
            ViewBag.Tipo = valor;
            List<Product> listaProductos = _contextDB.Product.ToList();
            Cookie();
            return View(listaProductos);
        }

        [HttpGet]
        public IActionResult ProductDetails(int valor)
        {
            ViewBag.ID = valor;
            var producto = _contextDB.Product.First(p => p.ID == valor);
            ViewBag.Existencia = producto.Stock;
            Cookie();
            return View(producto);
        }
    }
}
