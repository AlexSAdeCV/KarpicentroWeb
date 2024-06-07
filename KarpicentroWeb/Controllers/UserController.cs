using KarpicentroWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarpicentroWeb.Controllers
{
    public class UserController : Controller
    {
        public readonly KarpicentroDB _contextDB;
        public int ID { get; set; }

        public UserController(KarpicentroDB karpicentroDB)
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

        public IActionResult Welcome()
        {
            Cookie();
            return View();
        }

        [HttpGet]
        public IActionResult Boughts()
        {
            var listaventas = _contextDB.Cart.ToList();

            Cookie();

            return View(listaventas);
        }

        [HttpGet]
        public IActionResult Details(int id, int idord, string Shipping, int idudir)
        {
            Cookie();
            var user = _contextDB.User.FirstOrDefault(u => u.ID == id);
            var Udir = _contextDB.UseDir.FirstOrDefault(u => u.ID == idudir);

            ViewBag.Direccion = _contextDB.Direction.FirstOrDefault(u => u.ID == Udir.idDirections);
            ViewBag.Productos = _contextDB.Product.ToList();
            ViewBag.CarrVen = _contextDB.Cart.ToList();
            ViewBag.Inter = _contextDB.InterProd.ToList();
            ViewBag.NombreUser = user.Name;
            ViewBag.IDOrden = idord;
            ViewBag.Envio = Shipping;

            return View();
        }

        [HttpGet]
        public IActionResult Directions() 
        {
            Cookie();

            var listadireciones = _contextDB.Direction.ToList();
            var listainterdir = _contextDB.UseDir.Where(ud => ud.idUser == ID).ToList();

            ViewBag.userdir = listainterdir;

            return View(listadireciones);
        }

        [HttpGet]
        public IActionResult NewDirections()
        {
            Cookie();

            var listadireciones = _contextDB.Direction.ToList();
            var listainterdir = _contextDB.UseDir.Where(ud => ud.idUser == ID).ToList();

            ViewBag.userdir = listainterdir;

            return View(listadireciones);
        }

        [HttpPost]
        public IActionResult NewDirections(Directions directions)
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

            return RedirectToAction("Directions");
        }

        [HttpGet]
        public IActionResult EditDirections(int id)
        {
            Cookie();

            var direccion = _contextDB.Direction.FirstOrDefault(d => d.ID == id);

            return View(direccion);
        }

        [HttpPost]
        public IActionResult EditDirections(Directions directions)
        {
            Cookie();

            _contextDB.Direction.Update(directions);

            _contextDB.SaveChanges();

            return RedirectToAction("Directions");
        }

        [HttpGet]
        public IActionResult DeleteDirections(int? id)
        {
            Cookie();

            var direccion = _contextDB.Direction.FirstOrDefault(d => d.ID == id);

            return View(direccion);
        }

        [HttpPost]
        public IActionResult DeleteDirections(int id)
        {
            Cookie();

            var direccion = _contextDB.Direction.FirstOrDefault(d => d.ID == id);

            _contextDB.Direction.Remove(direccion);

            _contextDB.SaveChanges();

            return RedirectToAction("Directions");
        }
    }
}
