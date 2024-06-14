using KarpicentroWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarpicentroWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly KarpicentroDB _contextDB;

        public ValuesController(KarpicentroDB contexDB)
        {
            _contextDB = contexDB;
        }

        [HttpGet("ValorCombos")]
        public ActionResult<List<ComboBoxItem>> ValorCombos(int valor, int idprod)
        {
            var listproductoInter = _contextDB.InterProd.Where(p => p.idProducts == idprod).ToList();
            var listacolores = _contextDB.Colors.ToList();

            var coloresRelacionados = new HashSet<int>();

            foreach (var i in listproductoInter)
            {
                if (valor == i.idMaterials)
                {
                    coloresRelacionados.Add(i.idColors);
                }
            }

            listacolores = listacolores.Where(a => !coloresRelacionados.Contains(a.ID)).ToList();

            var items = listacolores.Select(a => new ComboBoxItem
            {
                Value = a.ID,
                Text = a.Name
            }).ToList();

            return items;
        }

        [HttpGet("DetalleCombos")]
        public ActionResult<DetalleCombosResponse> DetalleCombos(int valor, int idprod)
        {
            var listproductoInter = _contextDB.InterProd.Where(p => p.idProducts == idprod).ToList();
            var listacolores = _contextDB.Colors.ToList();

            var colores = new HashSet<int>();
            string imageUrl = null;

            foreach (var i in listproductoInter)
            {
                if (valor == i.idMaterials)
                {
                    colores.Add(i.idColors);
                    imageUrl = i.Image;
                }
            }

            listacolores = listacolores.Where(a => colores.Contains(a.ID)).ToList();

            var items = listacolores.Select(a => new ComboBoxItem
            {
                Value = a.ID,
                Text = a.Name,
                MaxQuantity = listproductoInter.FirstOrDefault(p => p.idMaterials == valor && p.idColors == a.ID)?.Stock ?? 0,
                ProductInterId = listproductoInter.FirstOrDefault(p => p.idMaterials == valor && p.idColors == a.ID)?.ID ?? 0,
                Price = listproductoInter.FirstOrDefault(p => p.idMaterials == valor && p.idColors == a.ID)?.SalePrice ?? 0  // Añadir el precio
            }).ToList();

            var response = new DetalleCombosResponse
            {
                Items = items,
                ImageUrl = imageUrl
            };

            return response;
        }

        [HttpGet("CambioCantidad")]
        public ActionResult<Response> CambioCantidad(int idprodInter, int valor, int carritoId)
        {
            var carrito = _contextDB.Cart.FirstOrDefault(i => i.ID == carritoId && i.idProductInter == idprodInter);
            var inter = _contextDB.InterProd.FirstOrDefault(i => i.ID == idprodInter);

            carrito.Amount = valor;
            carrito.Price = carrito.Amount * inter.SalePrice;

            _contextDB.Cart.Update(carrito);
            _contextDB.SaveChanges();

            var nuevototal = carrito.Amount * inter.SalePrice;

            var response = new Response {
                total = nuevototal
            };

            return response;
        }
    }

    public class Response
    {
        public decimal total { get; set; }
    }

    public class ComboBoxItem
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public int MaxQuantity { get; set; }
        public int ProductInterId { get; set; }
        public decimal Price { get; set; }  // Añadir el precio
    }

    public class DetalleCombosResponse
    {
        public List<ComboBoxItem> Items { get; set; }
        public string ImageUrl { get; set; }
    }
}
