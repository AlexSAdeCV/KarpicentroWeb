﻿using KarpicentroWeb.Helpers;
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
        public IActionResult NewSupplier()
        {
            Cookie();
            return View();
        }

        [HttpPost]
        public IActionResult NewSupplier(Supplier supplier)
        {
            Cookie();

            supplier.idDirections = 1;

            var productmodel = new ProductModel(_contextDB);

            productmodel.AddSupplier(supplier);

            return View();
        }

        [HttpGet]
        public IActionResult Features(int id, string nombre, string categorias)
        {
            List<ProductInter> listproductoInter = _contextDB.InterProd.Where(i => i.idProducts == id).ToList();
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

            ViewBag.idprod = idpro;

            Cookie();
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> AddFeatures(ProductInter productInter, IFormFile Image)
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
                ViewBag.Mensaje = "Seleccione una imagen para el producto";
                List<ProductInter> listproductoInter = _contextDB.InterProd.ToList();
                List<Colors> listproductoColor = _contextDB.Colors.ToList();
                List<Materials> listproductoMateriales = _contextDB.Materials.ToList();

                var viewmodel = new ProductViewModel
                {
                    Inter = listproductoInter,
                    Color = listproductoColor,
                    Material = listproductoMateriales
                };

                ViewBag.idprod = idpro;

                Cookie();
                return View(viewmodel);
            }

            productInter.Date = DateTime.Now;

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
        public IActionResult DeleteFeatures(int id)
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
        public IActionResult DeleteFeatures(ProductInter productInter)
        {
            ProductModel productoModel = new ProductModel(_contextDB);

            productoModel.DeleteFeatures(productInter);

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

        public IActionResult Charts()
        {
            Cookie();

            var coloresMasVendidos = (from cv in _contextDB.Cart
                                      join pci in _contextDB.InterProd on cv.idProductInter equals pci.ID
                                      join pc in _contextDB.Colors on pci.idColors equals pc.ID
                                      group cv by pc.Name into g
                                      select new
                                      {
                                          Color = g.Key,
                                          CantidadVendida = g.Sum(cv => cv.Amount)
                                      })
                             .OrderByDescending(g => g.CantidadVendida)
                             .ToList();

            var materialesMasComprados = (from cv in _contextDB.Cart
                                          join pci in _contextDB.InterProd on cv.idProductInter equals pci.ID
                                          join pa in _contextDB.Materials on pci.idMaterials equals pa.ID
                                          group cv by pa.Name into g
                                          select new
                                          {
                                              Materiales = g.Key,
                                              CantidadComprada = g.Sum(cv => cv.Amount)
                                          })
                                          .OrderByDescending(g => g.CantidadComprada)
                                          .ToList();

            var productosMasComprados = (from cv in _contextDB.Cart
                                         join pci in _contextDB.InterProd on cv.idProductInter equals pci.ID
                                         join p in _contextDB.Product on pci.idProducts equals p.ID
                                         group cv by p.Name into g
                                         select new
                                         {
                                             Producto = g.Key,
                                             CantidadComprada = g.Sum(cv => cv.Amount)
                                         })
                                         .OrderByDescending(g => g.CantidadComprada)
                                         .ToList();

            var directions = _contextDB.Direction.Skip(1).ToList(); // Omite el primer registro
            var stateGroups = directions.GroupBy(d => d.State).Select(g => new { State = g.Key, Count = g.Count() }).ToList();
            var delegationGroups = directions.GroupBy(d => d.Delegations).Select(g => new { Delegations = g.Key, Count = g.Count() }).ToList();
            var extIntNumbers = directions.Select(d => new { ExtNum = d.ExtNum, IntNum = d.IntNum }).ToList();

            var productPrices = (from pi in _contextDB.InterProd
                                 join p in _contextDB.Product on pi.idProducts equals p.ID
                                 select new
                                 {
                                     ProductName = p.Name,
                                     SalePrice = pi.SalePrice
                                 }).ToList();

            var stockByColor = (from pi in _contextDB.InterProd
                                join c in _contextDB.Colors on pi.idColors equals c.ID
                                group pi by c.Name into g
                                select new
                                {
                                    Color = g.Key,
                                    TotalStock = g.Sum(pi => pi.Stock)
                                }).ToList();

            var ventasPorDia = (from cb in _contextDB.Cart
                                group cb by cb.Date.Date into g
                                select new
                                {
                                    Fecha = g.Key,
                                    CantidadVentas = g.Count()
                                })
                                .OrderByDescending(g => g.CantidadVentas)
                                .ToList();

            var cartBuys = _contextDB.Cart.ToList(); // Carga los datos en memoria
            var promedioVentaPorMes = (from cb in cartBuys
                                       group cb by new { cb.Date.Year, cb.Date.Month } into g
                                       select new
                                       {
                                           Mes = new DateTime(g.Key.Year, g.Key.Month, 1),
                                           PromedioVenta = g.Average(x => x.Price * x.Amount)
                                       })
                                       .OrderBy(g => g.Mes)
                                       .ToList();

            ViewBag.Colores = coloresMasVendidos.Select(c => c.Color).ToList();
            ViewBag.CantidadesColores = coloresMasVendidos.Select(c => c.CantidadVendida).ToList();
            ViewBag.Materiales = materialesMasComprados.Select(a => a.Materiales).ToList();
            ViewBag.CantidadesMateriales = materialesMasComprados.Select(a => a.CantidadComprada).ToList();
            ViewBag.Productos = productosMasComprados.Select(p => p.Producto).ToList();
            ViewBag.CantidadesProductos = productosMasComprados.Select(p => p.CantidadComprada).ToList();

            ViewBag.States = stateGroups.Select(s => s.State).ToList();
            ViewBag.CountsStates = stateGroups.Select(s => s.Count).ToList();
            ViewBag.Delegations = delegationGroups.Select(d => d.Delegations).ToList();
            ViewBag.CountsDelegations = delegationGroups.Select(d => d.Count).ToList();
            ViewBag.ExtIntNumbers = extIntNumbers;

            ViewBag.ProductPrices = productPrices.Select(p => p.ProductName).ToList();
            ViewBag.SalePrices = productPrices.Select(p => p.SalePrice).ToList();
            ViewBag.Colors = stockByColor.Select(c => c.Color).ToList();
            ViewBag.TotalStock = stockByColor.Select(c => c.TotalStock).ToList();

            ViewBag.Days = ventasPorDia.Select(v => v.Fecha.ToString("yyyy-MM-dd")).ToList();
            ViewBag.VentasPorDia = ventasPorDia.Select(v => v.CantidadVentas).ToList();
            ViewBag.PromedioVentaMeses = promedioVentaPorMes.Select(p => p.Mes.ToString("yyyy-MM")).ToList();
            ViewBag.PromediosVenta = promedioVentaPorMes.Select(p => p.PromedioVenta).ToList();

            return View();
        }

    }
}
