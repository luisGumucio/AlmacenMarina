using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenMarina.Model;

namespace AlmacenMarina.Controls
{
    public class ControlSales
    {
        private List<ProductDetail> listProduct;
        private MarinaDbDataContext db;
        private String message = null;
        private decimal money = 0;

        public ControlSales()
        {
            listProduct = new List<ProductDetail>();
            db = new MarinaDbDataContext();
        }
        internal String addSales(string code)
        {

            if (validarProduct(code))
            {
                addProduct(code);
                return message;
            }
            return message = "no existe el producto";
        }

        internal ProductDetail ultimoSale()
        {
            return listProduct.LastOrDefault();
        }

        private bool validarProduct(String code)
        {
            return db.Product.Join(db.CodeProduct, p => p.IdProduct, c => c.IdProduct, (p, c) => new { p, c }).Where(p => p.c.IdCodeProduct.ToString() == code && p.c.Enable == true || p.c.IdCodeBox.ToString() == code && p.c.Enable == true).Count() != 0;
        }

        private ProductDetail products(string code)
        {
            ProductDetail result = new ProductDetail();
            var t = db.Product.Join(db.CodeProduct, p => p.IdProduct, c => c.IdProduct, (p, c) => new { p, c }).Where(b => b.c.IdCodeProduct.ToString() == code && b.c.Enable == true || b.c.IdCodeBox.ToString() == code && b.c.Enable == true).FirstOrDefault();
            if (t.c.IdCodeProduct.ToString() == code)
            {

                result.IdProducto = t.c.IdCodeProduct.Value;
                result.NombreProducto = t.p.nameProduct;
                result.Precio = t.p.price.Value;
                result.Cantidad = 1;
            }
            else
            {
                result.IdProducto = t.c.IdCodeBox;
                result.NombreProducto = t.p.nameProduct;
                decimal a = t.p.price.Value * t.c.Quality.Value;
                result.Precio = a;
                result.Cantidad = t.c.Quality.Value;
            }

            return result;
        }

        private String addProduct(string code)
        {
            if (listProduct.Count == 0)
            {
                listProduct.Add(products(code));
                money = money + products(code).Precio;
                message = "Habilitado";
            }
            else
            {
                //if (listProduct.Where(b => b.IdProducto.ToString() == code).Count() != 0)
                //{
                //    return message = "Esta en lista";

                //}
                //else
                //{
                    var t = db.CodeProduct.Where(b => b.IdCodeProduct.ToString() == code || b.IdCodeBox.ToString() == code).Count();
                    if (t != 0)
                    {
                        if (listProduct.Join(db.Product, l => l.NombreProducto, p => p.nameProduct, (l, p) => new { l, p })
                                   .Join(db.CodeProduct, b => b.p.IdProduct, c => c.IdProduct, (b, c) => new { b, c }).
                                   Where(w => w.c.IdCodeBox == w.b.l.IdProducto && w.c.IdCodeProduct.ToString() == code).Count() != 0)
                        {
                            return message = "Esta en lista";
                        }
                        else
                        {
                          if (listProduct.Join(db.Product, l => l.NombreProducto, p => p.nameProduct, (l, p) => new { l, p })
                                   .Join(db.CodeProduct, b => b.p.IdProduct, c => c.IdProduct, (b, c) => new { b, c }).
                                   Where(w => w.c.IdCodeProduct == w.b.l.IdProducto && w.c.IdCodeBox.ToString() == code).Count() != 0)
                            {
                                return message = "Esta en lista";
                            }
                          else
                          {
                              listProduct.Add(products(code));
                              money = money + products(code).Precio;
                              return message = "Habilitado";
                          }
                        }
                    }

                }


            return message;
        }

        public String Message()
        {
            return message;
        }

        internal void removeSalse(ProductDetail p)
        {
            listProduct.Remove(p);
        }

        internal decimal totalMoney()
        {
            return money;
        }

        internal void deleteProduct(ProductDetail p)
        {
            listProduct.Remove(p);
        }

        public bool registerSales(Sales sales)
        {
            try
            {
                addSalesProduc(sales);
                foreach (var item in listProduct)
                {
                    var t = db.CodeProduct.Where(b => b.IdCodeBox == item.IdProducto || b.IdCodeProduct == item.IdProducto).FirstOrDefault();
                    t.Quality = t.Quality - item.Cantidad;
                    db.SubmitChanges();
                    DetailSales sale = new DetailSales();
                    sale.IdProduct = t.IdProduct;
                    sale.IdSales = db.Sales.Max(b => b.IdSales);
                    sale.PriceSales = t.Product.price;
                    sale.Quality = item.Cantidad;
                    db.DetailSales.InsertOnSubmit(sale);
                    db.SubmitChanges();
                }
                listProduct.Clear();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        

        }
        private void addSalesProduc(Sales sales)
        {
            try
            {
                var t = db.Sales.Where(b => b.DateSales == sales.DateSales).Count();
                if (t == 0)
                {
                    db.Sales.InsertOnSubmit(sales);
                    db.SubmitChanges();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
