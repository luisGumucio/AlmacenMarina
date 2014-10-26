using System;
using System.Windows;
using AlmacenMarina.Model;
using AlmacenMarina.Controls;
using System.Linq;
using System.Windows.Input;

namespace AlmacenMarina
{
    public partial class BuyProduct
    {
        private MarinaDbDataContext db = new MarinaDbDataContext();
        private static ControlBuy control = new ControlBuy();
        private Guid idUser;
        public BuyProduct(Guid idUser)
        {
            this.InitializeComponent();
            this.idUser = idUser;
            loadDate();

            // A partir de este punto se requiere la inserción de código para la creación del objeto.
        }
        /// <summary>
        /// inizializa todos los datos que se va cargar en el sistema.
        /// </summary>
        private void loadDate()
        {
            try
            {
                CBCategory.ItemsSource = db.Category.Select(b => b.nameCategory);
                var t = from i in db.Product
                        select i;
                foreach (var item in t)
                {
                    TxtNameProduct.AddItem(new AutoCompleteEntry(item.nameProduct,item.nameProduct));
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// accion del boton para guardar los datos del productos y las compras.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (validar())
            {
                Product product = new Product();
                product.nameProduct = TxtNameProduct.Text;
                product.price = AmountPrice(decimal.Parse(TxtPriceSale.Text),decimal.Parse(TxtAmount.Text));

                product.IdCategory = db.Category.Where(b=>b.nameCategory == CBCategory.SelectedItem.ToString()).FirstOrDefault().IdCategory;
                Buy buy = new Buy();
                buy.DateBuy = DateTime.Now.Date;
                buy.IdUser = idUser;
                ControlBuy control = new ControlBuy();
                CodeProduct code = new CodeProduct();
                code.IdCodeBox = Convert.ToInt64(TxtCodPaquete.Text);
                code.IdCodeProduct = Convert.ToInt64(TxtCodProduct.Text);
                code.Enable = true;
                code.Quality = decimal.Parse(TxtQuality.Text);
                code.DateMaturity = CalDate.SelectedDate.Value.Date;
                if (control.addProduct(product, buy, decimal.Parse(TxtPriceSale.Text), code))
                {
                    MessageBox.Show("registro con exito","exito");
                    clearValue();
                }
                else
                {
                    MessageBox.Show("error en el sistema", "error Sistema", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Ingrese los datos por favor", "registro", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// valida todos los datos del formulario que este correcto para luego su registro correspondiente
        /// </summary>
        /// <returns> true si todo esta correcto  y false si algun de los datos no fue ingresado y esta en blanco
        /// </returns>
        private bool validar()
        {
            return !TxtNameProduct.Text.Equals("") && !CBCategory.SelectedIndex.Equals(-1) &&
                   !TxtPriceSale.Text.Equals("") && !TxtAmount.Text.Equals("") && !TxtQuality.Text.Equals("") &&
                   !CalDate.SelectedDate.Equals(null);
        }

        /// <summary>
        /// Ayuda a calcular la cantida que va contener cada producto segun el aumento que pone el cliente
        /// </summary>
        /// <param name="priceBuy"></param>
        /// <param name="priceAmmount"></param>
        /// <returns>retorna el valor indicado del producto</returns>
        private decimal AmountPrice(decimal priceBuy, decimal priceAmmount)
        {
            decimal result = decimal.Parse(TxtQuality.Text) * priceBuy;
            decimal aumont = decimal.Parse(TxtQuality.Text) * priceAmmount;
            result = result + aumont;
            result = result / decimal.Parse(TxtQuality.Text);
            return result;
        }
        
        /// <summary>
        /// limpia todos los valores que esta en el formulario para su luego añadido correspondiente 
        /// </summary>
        private void clearValue()
        {
            TxtAmount.Clear();
            TxtCodPaquete.Clear();
            TxtCodProduct.Clear();
            TxtPriceSale.Clear();
            TxtQuality.Clear();
        }

        private void TxtCodProduct_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            keyNumber(sender, e);
        }

        private void TxtCodProduct_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void keyNumber(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.OemComma)
            {

            }
            else
            {
                e.Handled = true;
            }
        }

        private void TxtCodPaquete_KeyDown(object sender, KeyEventArgs e)
        {
            keyNumber(sender, e);
        }

        private void TxtPriceSale_KeyDown(object sender, KeyEventArgs e)
        {
            keyNumber(sender, e);
        }

        private void TxtAmount_KeyDown(object sender, KeyEventArgs e)
        {
            keyNumber(sender, e);
        }

        private void TxtQuality_KeyDown(object sender, KeyEventArgs e)
        {
            keyNumber(sender, e);
        }

    }
}