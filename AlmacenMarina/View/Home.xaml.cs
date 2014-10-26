using System;
using System.Windows;
using System.Linq;
using AlmacenMarina.Model;
using AlmacenMarina.View;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AlmacenMarina
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private String userName;
        private Guid id;
        private MarinaDbDataContext db = new MarinaDbDataContext();

        public Home(String userName)
        {
            this.userName = userName;
            this.InitializeComponent();
            lblUserActivo.Content = userName;
            id = db.User.Where(b => b.UserName == userName).FirstOrDefault().IdUser;
            // A partir de este punto se requiere la inserción de código para la creación del objeto.
        }
        public String UserNam()
        {
            return userName;
        }
        private void btClosed_Click(object sender, RoutedEventArgs e)
        {
            Application current = Application.Current;
            current.Shutdown();
        }

        private void btMinize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ItemSales_Click(object sender, RoutedEventArgs e)
        {
            frmContent.NavigationService.RemoveBackEntry();
            SalesProduct sales = new SalesProduct();
            frmContent.Content = sales;
        }

        private void ItemUser_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            frmContent.NavigationService.RemoveBackEntry();
            UserCreate user = new UserCreate(id);
            frmContent.Content = user;
        }

        private void ItemBuy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmContent.NavigationService.RemoveBackEntry();
                BuyProduct buy = new BuyProduct(id);
                frmContent.Content = buy;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ItemReport_Click(object sender, RoutedEventArgs e)
        {
            //frmContent.NavigationService.RemoveBackEntry();
            //BuyReport buy = new BuyReport();
            //frmContent.Content = buy;
            MenuItem v = new MenuItem();
            v.Header = "ejemplo";
            v.Height = 26;
            v.Width = 171;
            BitmapImage s = new BitmapImage();
            s.BeginInit();
            s.UriSource = new Uri(@"/AlmacenMarina;component/Imagenes/Iconos/home.png",UriKind.RelativeOrAbsolute);
            s.EndInit();
            Image r= new Image();
            r.Height = 20;
            r.Width = 20;
            r.Source = s;
            v.Icon = r;
            menuDate.Items.Add(v);
            menuDate.Items.Add("EJEMPLO1");
            menuDate.Items.Add("EJEMPLO2");
            menuDate.Items.Add("EJEMPLO3");
        }

        private void menuItem_Click(object sender, RoutedEventArgs e)
        {
            frmContent.NavigationService.RemoveBackEntry();
            SaleReport sale = new SaleReport();
            frmContent.Content = sale;
        }

    }
}