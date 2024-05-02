using KarpicentroWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace KarpicentroWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KarpicentroDB _contextDB;

        public HomeController(ILogger<HomeController> logger, KarpicentroDB contextDB)
        {
            _logger = logger;
            _contextDB = contextDB;
        }

        public IActionResult Index()
        {
            Initialize();

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

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            UserModel login = new UserModel(_contextDB);

            login.Email = user.Email;
            login.Password = user.Password;

            if (login.Login())
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(365);
                options.IsEssential = true;
                options.Path = "/";
                HttpContext.Response.Cookies.Append("MiCookie", user.Email, options);

                return RedirectToAction("Index");
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user, string Password2)
        {
            UserModel register = new UserModel(_contextDB);

            register.Email = user.Email;
            register.Password = user.Password;
            register.Password2 = Password2;
            UserModel.Name = user.Name;

            if (register.Register())
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(365);
                options.IsEssential = true;
                options.Path = "/";
                HttpContext.Response.Cookies.Append("MiCookie", user.Email, options);

                if (register.Login())
                    return RedirectToAction("Index");
            }
            else
                ViewBag.Mensaje = UserModel.Message;

            return View(user);
        }

        [HttpGet]
        public IActionResult CerrarSesion()
        {
            HttpContext.Response.Cookies.Delete("MiCookie");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void Initialize()
        {
            _contextDB.Database.EnsureCreated();

            if (_contextDB.User.Any())
            {
                return;
            }

            var insertarusuarios = new User[]
                {
                    new User() {Email = "aserranoacosta841@gmail.com", Password = "1234", UserType = "Admin", Image = "../Images/Users/Alejandro.jpg", Name = "Alejandro"}
                };

            var insertardireccion = new Directions[]
                {
                    new Directions() {NameUser = "Prueba", Street = "Prueba", Neighborhood = "Prueba", ZipCode = "00000", Delegations = "Prueba", State = "Prueba", idUser = 1}
                };

            var insertarproveedor = new Supplier[]
                {
                    new Supplier() {NameSupplier = "Prueba", Active = 1, NameContact = "Prueba", Email = "example@gmail.com", idDirections = 1}
                };

            var insertarproductos = new Product[]
                {
                    new Product() { Name = "Sala orion", ProductFeatures = "Sala color gris", Description = "Bonita sala en color azul claro que le da un aspecto fresco a sus espacios, con sus acojinados de alta densidad y decorativos con motivos de palmeras.", PurchaseProduct = 1500, SalePrice = 7800, Width = 150, Height = 92, Depth = 92, Stock = 10, Categories = "Salas", Image = "../Images/Products/Sala.jpg", idSupplier = 1, Active = 1},
                    new Product() { Name = "Cocina Ontaro", ProductFeatures = "Sala color chocolate", Description = "Bonita cocina en color chocolate con acabado rallado, especiero inferior y manijas plateadas", PurchaseProduct = 3000, SalePrice = 12800, Width = 90, Height = 92, Depth = 66, Stock = 22, Categories = "Cocinas", Image = "../Images/Products/cocina.jpg", idSupplier = 1, Active = 1},
                    new Product() { Name = "Cabecera España", ProductFeatures = "Cabecera color gris", Description = "Bonita recámara con grabados transversales en abanico, burós en blanco con gris, con espacio inferior y manijas rectangular, que le dará un toque diferente a tus sueños.", PurchaseProduct = 600, SalePrice = 3400, Width = 8, Height = 2, Depth = 1, Stock = 18, Categories = "Recamaras", Image = "../Images/Products/Recamara.jpg", idSupplier = 1, Active = 1}
                };

            foreach (var u in insertarusuarios)
                _contextDB.User.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertardireccion)
                _contextDB.Direction.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarproveedor)
                _contextDB.Supplier.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarproductos)
                _contextDB.Product.Add(u);

            _contextDB.SaveChanges();
        }
    }
}
