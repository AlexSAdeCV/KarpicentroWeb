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
        public static int idpro { get; set; }
        public static string envio { get; set; }

        public AdminController(KarpicentroDB contextDB, HelperUploadFiles helperUpload)
        {
            _contextDB = contextDB;
            this._helperUpload = helperUpload;
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

        [HttpGet]
        public IActionResult Warehouse()
        {
            var listaproducto = _contextDB.Product.ToList();
            Cookie();
            return View(listaproducto);
        }

        [HttpGet]
        public IActionResult NewColor()
        {
            var listaproveedores = _contextDB.Supplier.ToList();
            Cookie();
            return View(listaproveedores);
        }

        [HttpPost]
        public IActionResult NewColor(Colors colors)
        {
            var productmodel = new ProductModel(_contextDB);

            productmodel.AddColor(colors);

            Cookie();
            return RedirectToAction("Warehouse");
        }

        [HttpGet]
        public IActionResult NewMaterial()
        {
            var listaproveedores = _contextDB.Supplier.ToList();
            Cookie();
            return View(listaproveedores);
        }

        [HttpPost]
        public IActionResult NewMaterial(Materials materials)
        {
            var productmodel = new ProductModel(_contextDB);

            productmodel.AddMaterial(materials);

            Cookie();
            return RedirectToAction("Warehouse");
        }

        [HttpGet]
        public IActionResult NewProduct()
        {
            var listaproveedores = _contextDB.Supplier.ToList();
            Cookie();
            return View(listaproveedores);
        }

        [HttpPost]
        public IActionResult NewProduct(Product product)
        {
            var productmodel = new ProductModel(_contextDB);

            productmodel.Add(product);

            Cookie();
            return RedirectToAction("Warehouse");
        }

        [HttpGet]
        public IActionResult Features(int id, string nombre, string categorias)
        {
            List<ProductInter> listproductoInter = _contextDB.InterProd.ToList();
            List<Colors> listproductoColor = _contextDB.Colors.ToList();
            List<Materials> listproductoMateriales = _contextDB.Materials.ToList();

            idpro = id;

            ViewBag.ID = idpro;

            var viewmodel = new ProductViewModel
            {
                Inter = listproductoInter,
                Color = listproductoColor,
                Material = listproductoMateriales
            };

            Cookie();
            ViewBag.Categoria = categorias;
            return View(viewmodel);
        }

        [HttpGet]
        public IActionResult AddFeatures()
        {
            List<ProductInter> listproductoInter = _contextDB.InterProd.ToList();
            List<Colors> listproductoColor = _contextDB.Colors.ToList();
            List<Materials> listproductoMateriales = _contextDB.Materials.ToList();

            var viewmodel = new ProductViewModel
            {
                Inter = listproductoInter,
                Color = listproductoColor,
                Material = listproductoMateriales
            };

            Cookie();
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> AddFeatures(ProductInter productInter, IFormFile Image)
        {
            ProductModel productoModel = new ProductModel(_contextDB);
            productInter.idProducts = idpro;

            string nombreImagen = +productInter.idProducts + "_" + productInter.idColors + "_" + productInter.idMaterials + "_" + Image.FileName;
            await this._helperUpload.UploadFilesAsync(Image, nombreImagen, Folders.Products);

            productInter.Image = "../Images/Products/" + nombreImagen;
            productoModel.AddFeatures(productInter);
            return RedirectToAction("Warehouse");
        }

        [HttpGet]
        public IActionResult EditFeatures(int id)
        {
            var productoInter = _contextDB.InterProd.FirstOrDefault(i => i.ID == id);
            List<Colors> listproductoColor = _contextDB.Colors.ToList();
            List<Materials> listproductoMateriales = _contextDB.Materials.ToList();

            var viewmodel = new ProductViewModel
            {
                pinter = productoInter,
                Color = listproductoColor,
                Material = listproductoMateriales

            };

            PfImage = productoInter.Image;

            Cookie();
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> EditFeatures(ProductInter productInter, IFormFile Image)
        {
            ProductModel productoModel = new ProductModel(_contextDB);
            productInter.idProducts = idpro;

            if (Image != null)
            {
                string nombreImagen = +productInter.idProducts + "_" + productInter.idColors + "_" + productInter.idMaterials + "_" + Image.FileName;
                await this._helperUpload.UploadFilesAsync(Image, nombreImagen, Folders.Products);

                productInter.Image = "../Images/Products/" + nombreImagen;
            }
            else
            {
                productInter.Image = PfImage;
            }

            productoModel.EditFeatures(productInter);
            return RedirectToAction("Warehouse");
        }

        [HttpGet]
        public IActionResult Boughts()
        {
            var listaventas = _contextDB.Cart.ToList();

            Cookie();

            return View(listaventas);
        }

        [HttpGet]
        public IActionResult ShippingDetails(int id, int idord, string Shipping, int idudir)
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

        [HttpPost]
        public IActionResult ShippingDetails(int id, int idord)
        {
            var orden = _contextDB.Cart.Where(c => c.IDOrder == idord).ToList();

            for (int i = 0; i < orden.Count; i++)
            {
                if (id == 1)
                {
                    orden[i].Shipping = "Tu paquete esta listo para ser enviado";
                }
                else if (id == 2)
                {
                    orden[i].Shipping = "Tu paquete esta en camino";
                }
                else if (id == 3)
                {
                    orden[i].Shipping = "Tu paquete a sido entregado";
                }
            }

            foreach (var u in orden)
                _contextDB.Cart.Update(u);
            _contextDB.SaveChanges();

            return RedirectToAction("Boughts");
        }

        [HttpGet]
        public IActionResult Sales()
        {
            var listaventas = _contextDB.Cart.ToList();

            Cookie();

            return View(listaventas);
        }
    }
}
