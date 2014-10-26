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
                return db.User.Where(b => b.UserName == user.UserName && b.Paswrod == user.Paswrod).FirstOrDefault().UserRol.FirstOrDefault().Roles.NameRol;
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

    }
}
