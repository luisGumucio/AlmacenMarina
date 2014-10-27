using AlmacenMarina.Controls;
using AlmacenMarina.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace AlmacenMarina.View
{
    /// <summary>
    /// Lógica de interacción para SalesProduct.xaml
    /// </summary>
    public partial class SalesProduct : Page
    {
        private ControlSales control = new ControlSales();
        private ProductDetail r = new ProductDetail();
        public SalesProduct()
        {
            InitializeComponent();
            TxtCodBarra.Focus();

        }
        private bool validate()
        {
            return !TxtCodBarra.Text.Equals("");
        }
        private bool habilitado()
        {
            switch (control.addSales(TxtCodBarra.Text))
            {
                case "Habilitado":
                    DatGridContent.Items.Add(control.ultimoSale());
                    TxtCodBarra.Clear();
                    LblTotVenta.Content = control.totalMoney().ToString();
                    return true;
            }
            return false;
        }
        private void TxtCodBarra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Monitor.Enter(this);
                {
                    if (validate())
                    {
                        if (!habilitado())
                        {
                            MessageBox.Show(control.Message());
                            Monitor.Exit(this);
                            TxtCodBarra.Clear();
                        }
                    }
                    else
                    {
                        MessageBox.Show("error ingrese el codigo por favor");
                        Monitor.Exit(this);
                    }
                }

            }
        }

        private void BtnCalcuVenta_Click(object sender, RoutedEventArgs e)
        {
            decimal a = decimal.Parse(TxtEfectivo.Text);
            decimal b = decimal.Parse(LblTotVenta.Content.ToString());
            decimal result =  a -  b;
            LblCambio.Content = result.ToString(); 
        }

        private void btEliminar_Click(object sender, RoutedEventArgs e)
        {
            ProductDetail t = new ProductDetail();
            for (int i = 0; i <= DatGridContent.SelectedItems.Count; i++)
            {
                t = (ProductDetail)DatGridContent.SelectedItems[i];
                DatGridContent.Items.Remove(DatGridContent.SelectedItems[i]);
                control.deleteProduct(t);
                LblTotVenta.Content = decimal.Parse(LblTotVenta.Content.ToString()) - t.Precio;
            };
        }

        private void btGuardar_Click(object sender, RoutedEventArgs e)
        {
            Sales salse = new Sales();
            salse.DateSales = DateTime.Now.Date;
            if (control.registerSales(salse))
            {
                MessageBox.Show("registro con existe");
                DatGridContent.Items.Clear();
            }
            else
            {
                MessageBox.Show("error al registrar");
            }
        }

        private void DatGridContent_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
        }
    }
}
