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

        public HomeController(ILogger<HomeController> logger, KarpicentroDB contextDB)
        {
            _logger = logger;
            _contextDB = contextDB;
        }

        public IActionResult Index()
        {
            Initialize();

            ViewBag.Nivel = _Nivel;

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
                return RedirectToAction("Index");
            }

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

            var insertardireccion = new Direccion[]
                {
                    new Direccion() {Nombre = "Prueba", ApellidoPaterno = "Prueba", ApellidoMaterno = "Prueba", Calle = "Prueba", Colonia = "Prueba", CodigoPostal = "00000", Delegacion = "Prueba"}
                };

            var insertarusuarios = new Usuario[]
                {
                    new Usuario() {Correo = "aserranoacosta841@gmail.com", Contrasena = "1234", TipoUsuario = "Admin", iddireccion = 1}
                };

            foreach (var u in insertardireccion)
                _contextDB.Direccion.Add(u);

            foreach (var u in insertarusuarios)
                _contextDB.Usuario.Add(u);

            _contextDB.SaveChanges();
        }
    }
}
