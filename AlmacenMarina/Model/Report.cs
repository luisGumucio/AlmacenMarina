using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlmacenMarina.Model
{
    public class Report
    {

        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public DateTime Fecha { get; set; }

    }
}
