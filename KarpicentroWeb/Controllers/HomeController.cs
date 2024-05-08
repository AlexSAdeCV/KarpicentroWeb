using KarpicentroWeb.Models;
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
                    new User() {Email = "aserranoacosta841@gmail.com", Password = "1234", UserType = "Admin", Image = "../Images/Users/Alejandro.jpg", Name = "Alejandro"},
                    new User() {Email = "ricardo_138@outlook.com", Password = "1234", UserType = "Admin", Image = "../Images/Usuarios/Rich.jpg", Name = "Rich"},
                    new User() {Email = "a@gmail.com", Password = "1234", UserType = "Cliente", Image = "../Images/Usuarios/Usuario.jpg", Name = "A"}
                };

            var insertardireccion = new Directions[]
                {
                    new Directions() {NameUser = "Prueba", Street = "Prueba", Neighborhood = "Prueba", ZipCode = "00000", Delegations = "Prueba", State = "Prueba"}
                };

            var Insertarusdir = new UserDirections[]
                {
                    new UserDirections() { idDirections = 1, idUser = 1},
                    new UserDirections() { idDirections = 1, idUser = 2},
                    new UserDirections() { idDirections = 1, idUser = 3}
                };

            var insertarproveedor = new Supplier[]
                {
                    new Supplier() {NameSupplier = "Prueba", Active = 1, NameContact = "Prueba", Email = "example@gmail.com", idDirections = 1}
                };

            var insertarproductos = new Product[]
                {
                    new Product() { Name = "Sala orion", ProductFeatures = "Sala color gris", Description = "Bonita sala en color azul claro que le da un aspecto fresco a sus espacios, con sus acojinados de alta densidad y decorativos con motivos de palmeras.", PurchasePrice = 1500, Width = 150, Height = 92, Depth = 92, Categories = "Salas", idSupplier = 1, Active = 1},
                    new Product() { Name = "Cocina Ontaro", ProductFeatures = "Sala color chocolate", Description = "Bonita cocina en color chocolate con acabado rallado, especiero inferior y manijas plateadas", PurchasePrice = 3000, Width = 90, Height = 92, Depth = 66, Categories = "Cocinas", idSupplier = 1, Active = 1},
                    new Product() { Name = "Cabecera España", ProductFeatures = "Cabecera color gris", Description = "Bonita recámara con grabados transversales en abanico, burós en blanco con gris, con espacio inferior y manijas rectangular, que le dará un toque diferente a tus sueños.", PurchasePrice = 600, Width = 8, Height = 2, Depth = 1, Categories = "Recamaras", idSupplier = 1, Active = 1}
                };

            var insertarcolores = new Colors[]
                {
                    new Colors() { Name = "Negro"},
                    new Colors() { Name = "Azul"},
                    new Colors() { Name = "Rojo"},
                    new Colors() { Name = "Cafe"},
                    new Colors() { Name = "Blanco"}
                };

            var insertarmateriales = new Materials[]
                {
                    new Materials() {Name = "Piel"},
                    new Materials() { Name = "Madera"}
                };

            var insertarintermedia = new ProductInter[]
                {
                    new ProductInter() { idProducts = 1, idColors = 1, idMaterials = 1, Stock = 15, SalePrice = 2200, Image = "../Images/Products/Sala.jpg"},
                    new ProductInter() { idProducts = 2, idColors = 4, idMaterials = 2, Stock = 15, SalePrice = 2200, Image = "../Images/Products/cocina.jpg"},
                    new ProductInter() { idProducts = 3, idColors = 5, idMaterials = 2, Stock = 15, SalePrice = 2200, Image = "../Images/Products/Recamara.jpg"}
                };

            foreach (var u in insertarusuarios)
                _contextDB.User.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertardireccion)
                _contextDB.Direction.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in Insertarusdir)
                _contextDB.UseDir.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarproveedor)
                _contextDB.Supplier.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarproductos)
                _contextDB.Product.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarcolores)
                _contextDB.Colors.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarmateriales)
                _contextDB.Materials.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarintermedia)
                _contextDB.InterProd.Add(u);

            _contextDB.SaveChanges();
        }
    }
}
