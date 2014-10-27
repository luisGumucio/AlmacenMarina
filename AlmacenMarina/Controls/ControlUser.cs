using System;
using System.Linq;
using AlmacenMarina.Model;
namespace AlmacenMarina.Controls
{
    /// <summary>
    /// encargado para el control de usuario tanto.
    /// </summary>
    public class ControlUser
    {
        private static MarinaDbDataContext db = new MarinaDbDataContext();
        private String message;
        /// <summary>
        /// Verifica que el usuario esta correcto para ingresar al sistema.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>lanza una execption si no existe el usuario o algun error</returns>
        public String Acceso(User user)
        {
            try
            {
               var t= db.User.ToList();
               String rol = "";
                foreach (var item in t)
                {
                    if (item.UserName == user.UserName && user.Paswrod.Equals(Desepcritar(item.Paswrod)))
                    {
                        rol=item.UserRol.Select(b => b.Roles).FirstOrDefault().NameRol;
                        break;
                    }
                }
                return rol;
            }
            catch (Exception)
            {

                return null;
            }

        }

        /// <summary>
        /// Crea un nuevo usuario y lo registra en la base de datos.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True si todo esta correcto</returns>
        public bool CreateUser(PersonUser user)
        {
            try
            {
                if (ValidateUser(user.User))
                {
                    db.User.InsertOnSubmit(user.User);
                    db.SubmitChanges();
                    user.UserPers.IdUser = user.User.IdUser;
                    user.UserPers.IdRol = db.Roles.Where(b => b.NameRol == user.Roles.NameRol).FirstOrDefault().IdRol;
                    db.UserRol.InsertOnSubmit(user.UserPers);
                    db.SubmitChanges();
                    db.Person.InsertOnSubmit(user.Person);
                    db.SubmitChanges();
                }
                else
                {
                    message = "el usuario ya existe";
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                message = "Error al registrar";
                return false;
            }
      

        }

        private bool ValidateUser(User user)
        {
            return db.User.Where(b => b.UserName == user.UserName).Count() == 0;
        }

        public String Message()
        {
            return message;
        }

        private String Desepcritar(String contraseña)
        {
            string[] vlEnEncrypt = contraseña.Split('/');


            string[] vlEncode = vlEnEncrypt[0].Split(',');
            string[] vlKey = vlEnEncrypt[1].Split(',');
            string[] vlIV = vlEnEncrypt[2].Split(',');

            //Creamos las variables byte[] y le creamos el tamaño 
            byte[] vlByEncode = new byte[vlEncode.Length];
            byte[] vlByKey = new byte[vlKey.Length];
            byte[] vlByIV = new byte[vlIV.Length];

            //Variable int que almacena la cantidad de item del array
            int vlConEncode = vlEncode.Length - 1;
            //Recorremos el string[] y le pasamos el valor al byte[]
            for (int i = 0; i <= vlConEncode; i++)
            {
                vlByEncode[i] = byte.Parse(vlEncode[i]);
            }

            //Variable int que almacena la cantidad de item del array
            int vlConKey = vlKey.Length - 1;
            //Recorremos el string[] y le pasamos el valor al byte[]
            for (int i = 0; i <= vlConKey; i++)
            {
                vlByKey[i] = byte.Parse(vlKey[i]);
            }

            //Variable int que almacena la cantidad de item del array
            int vlConIV = vlEncode.Length - 1;
            //Recorremos el string[] y le pasamos el valor al byte[]
            for (int i = 0; i <= vlConIV; i++)
            {
                vlByIV[i] = byte.Parse(vlIV[i]);
            }

            //Mandamos a desencriptar el texto
            string vlValue = RijndaelEncrypt.Decode(vlByEncode, vlByKey, vlByIV).ToString();

            return vlValue;
        }

    }
}
