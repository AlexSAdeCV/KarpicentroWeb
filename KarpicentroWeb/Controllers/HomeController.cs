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

            var inter = _contextDB.InterProd.OrderByDescending(c => c.Date).ToList();

            List<ProductInter> lista = new List<ProductInter>();

            var y = 0;

            foreach (var i in inter)
            {
                lista.Add(i);
                y++;

                if (y == 3)
                    break;
            }

            var producto = _contextDB.Product.ToList();
            var color = _contextDB.Colors.ToList();
            var material = _contextDB.Materials.ToList();

            var model = new ProductViewModel
            {
                Inter = lista,
                Products = producto,
                Color = color,
                Material = material
            };

            return View(model);
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

        [HttpGet]
        public IActionResult Presentacion()
        {
            return View();
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
                    new User() {Email = "ricardo_138@outlook.com", Password = "1234", UserType = "Admin", Image = "../Images/Users/Rich.jpg", Name = "Rich"},
                    new User() {Email = "a@gmail.com", Password = "1234", UserType = "Cliente", Image = "../Images/Users/Usuario.png", Name = "A"}
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
                    new Product()
                    {
                        ID = 1,
                        Name = "Sala orion",
                        ProductFeatures = "Sala color gris",
                        Description = "Bonita sala en color azul claro que le da un aspecto fresco a sus espacios, con sus acojinados de alta densidad y decorativos con motivos de palmeras.",
                        PurchasePrice = 1500m,
                        Width = 150,
                        Height = 92,
                        Depth = 92,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 1
                    },
                    new Product()
                    {
                        ID = 2,
                        Name = "Cocina Ontaro",
                        ProductFeatures = "Sala color chocolate",
                        Description = "Bonita cocina en color chocolate con acabado rallado, especiero inferior y manijas plateadas",
                        PurchasePrice = 3000m,
                        Width = 90,
                        Height = 92,
                        Depth = 66,
                        Categories = "Cocinas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 1
                    },
                    new Product()
                    {
                        ID = 3,
                        Name = "Cabecera España",
                        ProductFeatures = "Cabecera color gris",
                        Description = "Bonita recámara con grabados transversales en abanico, burós en blanco con gris, con espacio inferior y manijas rectangular, que le dará un toque diferente a tus sueños.",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Recamaras",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 4,
                        Name = "Comedor Cristal",
                        ProductFeatures = "Comedor con sillas azules, mesa de cristal",
                        Description = "Comedor de 6 sillas, color azul turquesa, con mesa de cristal templado y soporte de madera",
                        PurchasePrice = 800m,
                        Width = 6,
                        Height = 4,
                        Depth = 3,
                        Categories = "Cocinas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 5,
                        Name = "Comedor Azul",
                        ProductFeatures = "Mesa Blanca y Sillas Azules",
                        Description = "Bonito comedor de 4 sillas color azul rey, con soporte de patas de madera",
                        PurchasePrice = 800m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "cocinas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 6,
                        Name = "Recamara azul",
                        ProductFeatures = "Cabecera azul rey",
                        Description = "Bonita recamara con cabecera color azul fuerte, con colchon blanco y almohadas blancas.",
                        PurchasePrice = 500m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Recamaras",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 7,
                        Name = "Recamara Azul Pastel",
                        ProductFeatures = "Cabecera azul pastel",
                        Description = "Bonita recamara con cabecera azul pastel, cama y almohadas blancas.",
                        PurchasePrice = 800m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Recamaras",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 8,
                        Name = "Recamara beige-Azul",
                        ProductFeatures = "Cabecera y cuarto azul rey",
                        Description = "Bonita recamara color azul fuerte, soporte del mismo y colchon blanco con almohadas blancas",
                        PurchasePrice = 900m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Recamaras",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 9,
                        Name = "Sala azul Rey",
                        ProductFeatures = "Duo de sillones azul rey",
                        Description = "Bonita sala de doble sillon, color azul rey de 4 lugares por los dos sillones",
                        PurchasePrice = 700m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 10,
                        Name = "Sala exterior",
                        ProductFeatures = "Sala de esquina de exteriores",
                        Description = "Bonita sala de exteriores, color azul turquesa, soporte de madera con mesa de te",
                        PurchasePrice = 800m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 11,
                        Name = "Sala de madera exterior",
                        ProductFeatures = "Sala de doble asiento azul",
                        Description = "Bonita silla de doble exterior de madera, color azul pastel",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 12,
                        Name = "Sala de madera exterior individual",
                        ProductFeatures = "Sala de asiento individual azul",
                        Description = "Bonita silla individual de exteriores, color azul turquesa",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 13,
                        Name = "Silla de madera azul",
                        ProductFeatures = "Silla de madera azul",
                        Description = "Bonita silla individual color azul.",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 14,
                        Name = "Silla de madera de metal",
                        ProductFeatures = "Silla de metal azul",
                        Description = "",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 15,
                        Name = "Sillon mediano",
                        ProductFeatures = "Sillon mediano azul",
                        Description = "Bonito sillon de doble asiento, color azul",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 16,
                        Name = "Sillon color azul fuerte",
                        ProductFeatures = "Sillon doble mediano azul",
                        Description = "Bonito sillon color azul fuerte, comodo y suave",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 17,
                        Name = "Sillon color azul turquesa",
                        ProductFeatures = "Sillon triple color azul turquesa",
                        Description = "Bonito sillon sillon de triple asiento, color azul turquesa",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 18,
                        Name = "Sillon individual azul pastel",
                        ProductFeatures = "Sillon individual color azul pastel",
                        Description = "Bonita silla individual azul turquesa",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 19,
                        Name = "Sillon corte redondo azul",
                        ProductFeatures = "Sillon de terciopelo azul",
                        Description = "Bonito sillon de terciopelo color azul oscuro",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 20,
                        Name = "Silla individual",
                        ProductFeatures = "Silla individual color azul",
                        Description = "Bonita silla individual color azul",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 21,
                        Name = "Sillon chico Individual",
                        ProductFeatures = "Sillon chico individual azul",
                        Description = ".",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 22,
                        Name = "Sillon chico individual color azul fuerte",
                        ProductFeatures = "Sillon chico individual",
                        Description = "Sillon chico individual",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 23,
                        Name = "Sillon doble mediano azul fuerte",
                        ProductFeatures = "Sillon doble azul fuerte",
                        Description = "Bonito sillon color azul",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    },
                    new Product()
                    {
                        ID = 24,
                        Name = "Sillon Individual Azul claro",
                        ProductFeatures = "Sillon individual azul",
                        Description = "Sillon azul",
                        PurchasePrice = 600m,
                        Width = 8,
                        Height = 2,
                        Depth = 1,
                        Categories = "Salas",
                        idSupplier = 1,
                        Active = 1,
                        Featured = 0
                    }
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

            var insertarintermedia = new List<ProductInter>
                {
                    new ProductInter
                    {
                        ID = 1,
                        idProducts = 1,
                        idColors = 1,
                        idMaterials = 1,
                        Stock = 15,
                        SalePrice = 2200m,
                        Image = "../Images/Products/Sala.jpg",
                        Date = new DateTime(2024, 6, 13, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 2,
                        idProducts = 2,
                        idColors = 4,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 4000m,
                        Image = "../Images/Products/cocina.jpg",
                        Date = new DateTime(2024, 6, 4, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 3,
                        idProducts = 3,
                        idColors = 5,
                        idMaterials = 1,
                        Stock = 15,
                        SalePrice = 15000m,
                        Image = "../Images/Products/Recamara.jpg",
                        Date = new DateTime(2024, 6, 14, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 4,
                        idProducts = 4,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 10000m,
                        Image = "../Images/Products/ComedorCristalSF.jpeg",
                        Date = new DateTime(2024, 6, 12, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 5,
                        idProducts = 5,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 14000m,
                        Image = "../Images/Products/ComedorMaderaSF.jpeg",
                        Date = new DateTime(2024, 6, 8, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 6,
                        idProducts = 6,
                        idColors = 2,
                        idMaterials = 1,
                        Stock = 15,
                        SalePrice = 5000m,
                        Image = "../Images/Products/RecamaraBue1SF.jpg",
                        Date = new DateTime(2024, 6, 17, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 7,
                        idProducts = 7,
                        idColors = 2,
                        idMaterials = 1,
                        Stock = 15,
                        SalePrice = 3000m,
                        Image = "../Images/Products/RecamaraBue2SF.jpg",
                        Date = new DateTime(2024, 6, 18, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 8,
                        idProducts = 8,
                        idColors = 2,
                        idMaterials = 1,
                        Stock = 15,
                        SalePrice = 14000m,
                        Image = "../Images/Products/recamaraSF.jpg",
                        Date = new DateTime(2024, 6, 25, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 9,
                        idProducts = 9,
                        idColors = 2,
                        idMaterials = 1,
                        Stock = 14,
                        SalePrice = 4000m,
                        Image = "../Images/Products/SaladedosSF.jpeg",
                        Date = new DateTime(2024, 6, 8, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 10,
                        idProducts = 10,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 7000m,
                        Image = "../Images/Products/SalaExteriorSF.jpg",
                        Date = new DateTime(2024, 6, 4, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 11,
                        idProducts = 11,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 6000m,
                        Image = "../Images/Products/SillaDOBLEExterior.jpg",
                        Date = new DateTime(2024, 6, 18, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 12,
                        idProducts = 12,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 1000m,
                        Image = "../Images/Products/SillaExteriorIndividualSF.jpg",
                        Date = new DateTime(2024, 6, 4, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 13,
                        idProducts = 13,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 1500m,
                        Image = "../Images/Products/SillaMadera SF.jpeg",
                        Date = new DateTime(2024, 6, 16, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 14,
                        idProducts = 14,
                        idColors = 2,
                        idMaterials = 1,
                        Stock = 15,
                        SalePrice = 2000m,
                        Image = "../Images/Products/SillaMetalSF.jpeg",
                        Date = new DateTime(2024, 6, 7, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 15,
                        idProducts = 15,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 7500m,
                        Image = "../Images/Products/Sillon2PcuadSF.jpeg",
                        Date = new DateTime(2024, 6, 17, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 16,
                        idProducts = 16,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 12000m,
                        Image = "../Images/Products/Sillon2PlazSF.jpeg",
                        Date = new DateTime(2024, 6, 8, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 17,
                        idProducts = 17,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 1000m,
                        Image = "../Images/Products/Sillon3PlazSF.jpeg",
                        Date = new DateTime(2024, 6, 13, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 18,
                        idProducts = 18,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 15000m,
                        Image = "../Images/Products/sillonchicbabSF.jpeg",
                        Date = new DateTime(2024, 6, 16, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 19,
                        idProducts = 19,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 4000m,
                        Image = "../Images/Products/SillonBrutasSF.jpeg",
                        Date = new DateTime(2024, 6, 17, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 20,
                        idProducts = 20,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 6000m,
                        Image = "../Images/Products/SillonchicmamaSF.jpeg",
                        Date = new DateTime(2024, 6, 22, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 21,
                        idProducts = 21,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 5000m,
                        Image = "../Images/Products/sillonchiquitoSF.jpeg",
                        Date = new DateTime(2024, 6, 6, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 22,
                        idProducts = 22,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 2000m,
                        Image = "../Images/Products/sillonchixpapaSF.jpeg",
                        Date = new DateTime(2024, 6, 13, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 23,
                        idProducts = 23,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 1500m,
                        Image = "../Images/Products/SILLONGODART.jpeg",
                        Date = new DateTime(2024, 6, 26, 15, 30, 0)
                    },
                    new ProductInter
                    {
                        ID = 24,
                        idProducts = 24,
                        idColors = 2,
                        idMaterials = 2,
                        Stock = 15,
                        SalePrice = 3500m,
                        Image = "../Images/Products/SillonMOREREALSF.jpeg",
                        Date = new DateTime(2024, 6, 21, 15, 30, 0)
                    }
            };


            // Venta = 2, Carrito = 1
            var insertarventas = new CartBuys[]
                {
                    new CartBuys() {Price = 15000, Amount  = 5, Date = new DateTime(2024, 6, 13, 15, 30, 0), SwitchCartBuy = 2, Shipping = "Preparando tu producto", IDOrder = 1, idProductInter = 1, idUser = 1, iduserdir = 1},
                    new CartBuys() {Price = 1500, Amount  = 3, Date = new DateTime(2024, 5, 8, 15, 30, 0), SwitchCartBuy = 2, Shipping = "Preparando tu producto", IDOrder = 2, idProductInter = 2, idUser = 2, iduserdir = 1},
                    new CartBuys() {Price = 1700, Amount  = 1, Date = new DateTime(2024, 2, 3, 15, 30, 0), SwitchCartBuy = 2, Shipping = "Preparando tu producto", IDOrder = 2, idProductInter = 1, idUser = 2, iduserdir = 1}
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

            foreach (var u in insertarventas)
                _contextDB.Cart.Add(u);

            _contextDB.SaveChanges();
        }
    }
}
