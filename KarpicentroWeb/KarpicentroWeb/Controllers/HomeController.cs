using KarpicentroWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KarpicentroWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KarpicentroDB _contextDB;
        public static string _Nivel;
        public static string _Imagen;
        public static string _Nombre;

        public HomeController(ILogger<HomeController> logger, KarpicentroDB contextDB)
        {
            _logger = logger;
            _contextDB = contextDB;
        }

        public IActionResult Index()
        {
            Initialize();

            ViewBag.Nivel = _Nivel;
            ViewBag.FotoPerfil = _Imagen;
            ViewBag.Nombre = _Nombre;

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
        public IActionResult Login(Usuario Usuario)
        {
            UsuarioModel login = new UsuarioModel(_contextDB);

            login.Correo = Usuario.Correo;
            login.Contrasena = Usuario.Contrasena;

            if (login.Login())
            {
                _Nivel = UsuarioModel.TipoUsuario;
                _Imagen = UsuarioModel.DireccionImagen;
                _Nombre = UsuarioModel.Nombre;
                return RedirectToAction("Index");
            }

            return View(Usuario);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Usuario Usuario, string Contrasena2)
        {
            UsuarioModel register = new UsuarioModel(_contextDB);

            register.Correo = Usuario.Correo;
            register.Contrasena = Usuario.Contrasena;
            register.Contrasena2 = Contrasena2;
            UsuarioModel.Nombre = Usuario.Nombre;

            if (register.Register())
            {
                _Nivel = UsuarioModel.TipoUsuario;
                _Imagen = UsuarioModel.DireccionImagen;
                _Nombre = UsuarioModel.Nombre;
                
                if (register.Login())
                    return RedirectToAction("Index");
            }
            else
                ViewBag.Mensaje = UsuarioModel.Mensaje;

            return View(Usuario);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void Initialize()
        {
            _contextDB.Database.EnsureCreated();

            if (_contextDB.Usuario.Any())
            {
                return;
            }

            var insertardireccion = new Direccion[]
                {
                    new Direccion() {Nombre = "Prueba", ApellidoPaterno = "Prueba", ApellidoMaterno = "Prueba", Calle = "Prueba", Colonia = "Prueba", CodigoPostal = "00000", Delegacion = "Prueba"}
                };

            var insertarusuarios = new Usuario[]
                {
                    new Usuario() {Correo = "aserranoacosta841@gmail.com", Contrasena = "1234", TipoUsuario = "Admin", iddireccion = 1, DireccionImagen = "../wwwroot/Images/Usuarios/Alejandro.jpg", Nombre = "Alejandro"}
                };

            foreach (var u in insertardireccion)
                _contextDB.Direccion.Add(u);

            foreach (var u in insertarusuarios)
                _contextDB.Usuario.Add(u);

            _contextDB.SaveChanges();
        }
    }
}
