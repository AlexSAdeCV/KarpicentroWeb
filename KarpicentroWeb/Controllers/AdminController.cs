using KarpicentroWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarpicentroWeb.Controllers
{
    public class AdminController : Controller
    {
        public readonly KarpicentroDB _contextDB;

        public AdminController(KarpicentroDB contextDB)
        {
            _contextDB = contextDB;
        }
        public IActionResult Welcome()
        {
            Cookie();
            return View();
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
    }
}
