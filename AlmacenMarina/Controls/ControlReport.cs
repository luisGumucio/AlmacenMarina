using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenMarina.Model;

namespace AlmacenMarina.Controls
{
    public class ControlReport
    {
        private static MarinaDbDataContext db;
        private List<Report> Rp;

        public ControlReport() 
        {
            db = new MarinaDbDataContext();
            Rp = new List<Report>();
        }

        public List<Report> retSale(String valor)
        {
            var t = db.Sales.Join(db.DetailSales, b => b.IdSales, d => d.IdSales, (b, d) => new { b, d }).Where(p => p.b.DateSales.Value.Month == fecha(valor));
            int y = 1;

            foreach (var item in t)
            {
                Report r = new Report();

                r.IdProducto = y;
                r.NombreProducto = item.d.Product.nameProduct;
                r.Precio = item.d.PriceSales.Value;
                r.Fecha = item.b.DateSales.Value.Date;

                Rp.Add(r);
                y++;

            }
            return Rp;
        }
        public List<Report> reptBuy (string valor)
        {
            var t=db.Buy.Join(db.DetailBuy,b=>b.IdBuy,d=>d.IdBuy,(b,d)=>new{b,d}).Where(p => p.b.DateBuy.Value.Month == fecha(valor));
            int y=1;

            foreach (var item in t)
	        {
                Report r=new Report();

                r.IdProducto=y;
                r.NombreProducto=item.d.Product.nameProduct;
                r.Precio = item.d.PriceBuy.Value;
                r.Fecha =item.b.DateBuy.Value.Date;

                Rp.Add(r);
                y++;
		        
	        }
            return Rp;
        
        }

        private int fecha(string value) 
        {
            switch (value)
            {
                case "Enero":
                return 01;
                case "Febrero":
                return 02;
                case "Marzo":
                return 03;
                case "Abril":
                return 04;
                case "Mayo":
                return 05;
                case "Junio":
                return 06;
                case "Julio":
                return 07;
                case "Agosto":
                return 08;
                case "Septiembre":
                return 09;
                case "Octubre":
                return 10;
                case "Noviembre":
                return 11;
                case "Diciembre":
                return 12;
                default :
                return -1;

            }

            }
        }
        
       
         
    
    }

