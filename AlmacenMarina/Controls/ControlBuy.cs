using AlmacenMarina.Model;
using System;
using System.Linq;

namespace AlmacenMarina.Controls
{
    public class ControlBuy
    {
        private static MarinaDbDataContext db;

        public ControlBuy()
        {
            db = new MarinaDbDataContext();
        }
        /// <summary>
        /// añade un producto a la base de datos.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="buy"></param>
        /// <param name="priceBuy"></param>
        /// <param name="code"></param>
        /// <returns>true si todo fue correcto y no existe ningun error</returns>
        public bool addProduct(Product product, Buy buy, decimal priceBuy, CodeProduct code)
        {
            try
            {
                var verificar = db.Product.Where(b => b.nameProduct == product.nameProduct).Count();
                if (verificar == 0)
                {
                    db.Product.InsertOnSubmit(product);
                    db.SubmitChanges();
                    code.IdProduct = db.Product.Where(b => b.nameProduct == product.nameProduct).FirstOrDefault().IdProduct;
                    db.CodeProduct.InsertOnSubmit(code);
                    db.SubmitChanges();
                    addDetailBuy(buy, priceBuy, code, product);
                }
                else
                {

                    Product t = db.Product.Where(b => b.nameProduct == product.nameProduct).FirstOrDefault();
                    t.price = product.price;
                    db.SubmitChanges();
                    var verifi = db.CodeProduct.Where(b =>b.IdCodeProduct == code.IdCodeProduct).Count();
                    if (verifi == 0)
                    {
                        code.IdProduct = t.IdProduct;
                        db.CodeProduct.InsertOnSubmit(code);
                        db.SubmitChanges();
                        addDetailBuy(buy, priceBuy, code, product);
                    }
                    else
                    {
                        CodeProduct produ = db.CodeProduct.Where(b => b.IdProduct == t.IdProduct).FirstOrDefault();
                        produ.Quality = produ.Quality + code.Quality;
                        db.SubmitChanges();
                        addDetailBuy(buy,priceBuy,code,product);
                    }
              
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        
        /// <summary>
        /// añade la compra del producto segun la fecha correspondiente
        /// </summary>
        /// <param name="buy"></param>
        private void addBuy(Buy buy)
        {
            try
            {
                var t = db.Buy.Where(b => b.DateBuy == buy.DateBuy).Count();
                if (t == 0)
                {
                    db.Buy.InsertOnSubmit(buy);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// añade los detalles de la compra del producto correspodiente.
        /// </summary>
        /// <param name="buy"></param>
        /// <param name="priceBuy"></param>
        /// <param name="code"></param>
        /// <param name="t"></param>
        private void addDetailBuy(Buy buy, decimal priceBuy, CodeProduct code, Product t)
        {
            addBuy(buy);
            DetailBuy detailBuy = new DetailBuy();
            detailBuy.IdBuy = db.Buy.Max(b => b.IdBuy);
            detailBuy.IdProduct = db.Product.Where(b =>b.nameProduct == t.nameProduct).FirstOrDefault().IdProduct;
            detailBuy.PriceBuy = priceBuy;
            detailBuy.Quality = code.Quality;
            db.DetailBuy.InsertOnSubmit(detailBuy);
            db.SubmitChanges();
        }
    }
}
