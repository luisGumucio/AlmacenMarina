using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AlmacenMarina.Controls;

namespace AlmacenMarina.View
{
    /// <summary>
    /// Lógica de interacción para BuyReport.xaml
    /// </summary>
    public partial class BuyReport : Page
    {
        ControlReport rp;
        public BuyReport()
        {
            InitializeComponent();
            this.cargar();
            rp = new ControlReport();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var item in rp.reptBuy(CbValue.SelectedItem.ToString()))
	        {
                DatGridContent.Items.Add(item);
	        } 

        }

        private void cargar() 
        {
            CbNom.Items.Add("Mes");
            CbNom.Items.Add("Año");

        }

        private void CbNom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbNom.SelectedItem.ToString() == "Mes")
            {
                cargarMes();
            }
            else
            {

            }
      
        }

        private void cargarMes()
        {
            CbValue.Items.Clear();
            CbValue.Items.Add("Enero");
            CbValue.Items.Add("Febrero");
            CbValue.Items.Add("Marzo");
            CbValue.Items.Add("Abril");
            CbValue.Items.Add("Mayo");
            CbValue.Items.Add("Junio");
            CbValue.Items.Add("Julio");
            CbValue.Items.Add("Agosto");
            CbValue.Items.Add("Septiembre");
            CbValue.Items.Add("Octubre");
            CbValue.Items.Add("Noviembre");
            CbValue.Items.Add("Diciembre");
        }



        
    }
}
