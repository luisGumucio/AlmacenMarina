using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AlmacenMarina.Model;
using AlmacenMarina.Controls;

namespace AlmacenMarina.View
{
    /// <summary>
    /// Interaction logic for UserCreate.xaml
    /// </summary>
    public partial class UserCreate : Page
    {
        private Guid idUser;
        private MarinaDbDataContext db = new MarinaDbDataContext();
        private ControlUser control = new ControlUser();
        public UserCreate(Guid idUser)
        {
            this.idUser = idUser;
            InitializeComponent();
            load();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Validate())
            {
                if (control.CreateUser(llenarUser()))
                {
                    MessageBox.Show("Registro con exito");
                    ClearValue();
                }
                else
                {
                    MessageBox.Show(control.Message(), "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Llene todos los datos por favor","Registro",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
        }
        
        /// <summary>
        /// cargar todos los datos que existe en la parte de usuario.
        /// </summary>
        private void load()
        {
            CbxRol.ItemsSource = db.Roles.Select(b => b.NameRol).ToList();
        }

        /// <summary>
        /// valida Toda la informacion que se mete en el formulario.
        /// </summary>
        /// <returns> true si todo esta correcto</returns>
        private bool Validate() 
        {
            return !TxtNombre.Text.Equals("") && !TxtApellido.Text.Equals("") &&
                   !TxtUsuario.Text.Equals("") && !TxtContaseña.Text.Equals("") &&
                   !txtAddres.Text.Equals("") && !txtCelu.Text.Equals("") && !txtCi.Text.Equals("")
                   && !CbxRol.SelectedIndex.Equals(-1);
        }

        /// <summary>
        /// llena todos los datos a la clase person user.
        /// </summary>
        /// <returns>retorna un person user</returns>
        private PersonUser llenarUser()
        {
            Guid id = new Guid();
            id = Guid.NewGuid();
            PersonUser result = new PersonUser();
            result.User.IdUser = id;
            result.User.UserName = TxtUsuario.Text;
            result.User.Paswrod = TxtContaseña.Text;
            result.Roles.NameRol = CbxRol.SelectedItem.ToString();
            result.Person.Name = TxtNombre.Text;
            result.Person.LastName = TxtApellido.Text;
            result.Person.Ci = Convert.ToInt32(txtCi.Text);
            result.Person.Telf = Convert.ToInt32(txtCelu.Text);
            result.Person.Addres = txtAddres.Text;
            result.Person.IdUser = id;

            return result;
        }


        /// <summary>
        /// limpia todos los datos del formulario despues de que fue registrado.
        /// </summary>
        private void ClearValue()
        {
            txtAddres.Clear();
            TxtApellido.Clear();
            txtCelu.Clear();
            txtCi.Clear();
            TxtContaseña.Clear();
            TxtNombre.Clear();
            TxtUsuario.Clear();
        }
    }
}
