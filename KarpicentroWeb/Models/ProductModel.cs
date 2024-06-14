namespace KarpicentroWeb.Models
{
    public class ProductModel
    {
        public readonly KarpicentroDB _contextDB;

        public ProductModel(KarpicentroDB karpicentroDB) 
        {
            _contextDB = karpicentroDB;
        }

        public void Add(Product product)
        {
            _contextDB.Product.Add(product);
            _contextDB.SaveChanges();
        }

        public void AddFeatures(ProductInter productInter)
        {
            _contextDB.InterProd.Add(productInter);
            _contextDB.SaveChanges();
        }

        public void EditFeatures(ProductInter productInter)
        {
            _contextDB.InterProd.Update(productInter);
            _contextDB.SaveChanges();
        }

        public void DeleteFeatures(ProductInter productInter)
        {
            _contextDB.InterProd.Remove(productInter);
            _contextDB.SaveChanges();
        }

        public void AddColor(Colors colors)
        {
            _contextDB.Colors.Add(colors);
            _contextDB.SaveChanges();
        }

        public void AddMaterial(Materials materials)
        {
            _contextDB.Materials.Add(materials);
            _contextDB.SaveChanges();
        }

        public void AddSupplier(Supplier supplier)
        {
            _contextDB.Supplier.Add(supplier);
            _contextDB.SaveChanges();
        }
    }
}
