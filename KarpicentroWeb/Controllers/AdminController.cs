using KarpicentroWeb.Helpers;
using KarpicentroWeb.Models;
using KarpicentroWeb.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace KarpicentroWeb.Controllers
{
    public class AdminController : Controller
    {
        public readonly KarpicentroDB _contextDB;
        private HelperUploadFiles _helperUpload;
        public static string PfImage { get; set; } = string.Empty;

        public AdminController(KarpicentroDB contextDB, HelperUploadFiles helperUpload)
        {
            _contextDB = contextDB;
            this._helperUpload = helperUpload;
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

        [HttpGet]
        public IActionResult Users()
        {
            var listausuarios = _contextDB.User.ToList();
            Cookie();
            return View(listausuarios);
        }

        [HttpGet]
        public IActionResult UserEdit(int valor)
        {
            var users = _contextDB.User.FirstOrDefault(u => u.ID == valor);
            Cookie();
            PfImage = users.Image;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(int valor, IFormFile Image, User user)
        {
            var usermodel = new UserModel(_contextDB);

            if (Image != null)
            {
                string Nombre = user.Name + Image.FileName;
                await this._helperUpload.UploadFilesAsync(Image, Nombre, Folders.Images);
                user.Image = "../Images/Users/" + Nombre;
            }
            else
            {
                user.Image = PfImage;
            }

            usermodel.Edit(user);

            Cookie();
            return RedirectToAction("Users");
        }

        [HttpGet]
        public IActionResult UserDelete(int valor)
        {
            var user = _contextDB.User.FirstOrDefault(u => u.ID == valor);
            Cookie();
            return View(user);
        }

        [HttpPost]
        public IActionResult UserDelete(int id, string correo)
        {
            var usermodel = new UserModel(_contextDB);
            var user = _contextDB.User.FirstOrDefault(u => u.ID == id);
            usermodel.Delete(user);
            Cookie();
            return RedirectToAction("Users");
        }
    }
}
