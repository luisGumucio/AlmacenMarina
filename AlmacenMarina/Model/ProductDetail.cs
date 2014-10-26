using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlmacenMarina.Model
{
    public class ProductDetail
    {
        private Int64 idProducto;
        private String nombreProducto;
        private decimal cantidad;
        private decimal precio;

        public ProductDetail()
        {
        }

        public decimal Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }


        public String NombreProducto
        {
            get { return nombreProducto; }
            set { nombreProducto = value; }
        }
        public Int64 IdProducto
        {
            get { return idProducto; }
            set { idProducto = value; }
        }

        public decimal Precio
        {
            get { return precio; }
            set { precio = value; }
        }

    }
}
