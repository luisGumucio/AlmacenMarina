using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AlmacenMarina.Model
{
    public class RijndaelEncrypt
    {
        /// <summary>
        /// Manda a encriptar un objecto
        /// </summary>
        /// <param name="pObject">Objecto a encriptar</param>
        /// <returns>Retorna el Objecto Encriptado con las Claves</returns>
        public static object Encode(object pObject)
        {
            //Creamos el algoritmo
            System.Security.Cryptography.Rijndael rijndael = System.Security.Cryptography.Rijndael.Create();

            //Almacenamos las claves
            object vlKey = rijndael.Key;
            object vlIV = rijndael.IV;

            //Usamos el algoritmo
            using (rijndael)
            {
                //Encriptamos el objecto
                object vlEncrypted = EncryptObjectToBytes(pObject, rijndael.Key, rijndael.IV);

                //Almacenamos en un string[] el objecto encritado
                string[] vlEnEncrypted = ((IEnumerable)vlEncrypted).Cast<object>()
                    .Select(x => x.ToString())
                    .ToArray();

                //Almacenamos en un string[] la clave 1
                string[] vlEnKey = ((IEnumerable)vlKey).Cast<object>()
                    .Select(x => x.ToString())
                    .ToArray();

                //Almacenamos en un string[] la clave 2
                string[] vlEnIV = ((IEnumerable)vlIV).Cast<object>()
                    .Select(x => x.ToString())
                    .ToArray();

                //Creamos el string con los datos encriptados
                string vlValueEncode = string.Empty;
                vlValueEncode = vlValueEncode + string.Join(",", vlEnEncrypted);
                vlValueEncode = vlValueEncode + "/" + string.Join(",", vlEnKey);
                vlValueEncode = vlValueEncode + "/" + string.Join(",", vlEnIV);

                //Almacenamos en un objecto el string 
                object vlValueFinal = vlValueEncode;

                //Retornamos el objecto
                return vlValueFinal;
            }

        }

        /// <summary>
        /// Manda a desencriptar un objecto
        /// </summary>
        /// <param name="pEncode">Objecto Encriptado</param>
        /// <param name="pKey">Clave 1 Encriptada</param>
        /// <param name="pIV">Clave 2 Encriptada</param>
        /// <returns>Retorna el Objecto Desencriptado</returns>
        public static object Decode(byte[] pEncode, byte[] pKey, byte[] pIV)
        {
            //Mandamos a desencriptar el objecto
            object vlValue = DecryptObjectFromBytes(pEncode, pKey, pIV);

            //Si el objecto no es nulo
            if (vlValue != null)
            {
                //Retornamos el objecto
                return vlValue;
            }
            return null;
        }

        /// <summary>
        /// Encriptamos el objecto
        /// </summary>
        /// <param name="pObject">Objecto a Encriptar</param>
        /// <param name="pKey">Clave 1</param>
        /// <param name="pIV">Clave 2</param>
        /// <returns>Retorna el objecto Encriptado</returns>
        private static byte[] EncryptObjectToBytes(object pObject, byte[] pKey, byte[] pIV)
        {
            //Verifica si los parametros son nulo
            if (pObject == null)
            {
                throw new ArgumentNullException();
            }
            if (pKey == null || pKey.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (pIV == null || pIV.Length <= 0)
            {
                throw new ArgumentNullException();
            }

            //Creamos la variable byte[] que almacena el objecto encriptado
            byte[] encrypted;

            //Creamos el algoritmo
            System.Security.Cryptography.Rijndael rijndael = System.Security.Cryptography.Rijndael.Create();

            //Usamos el algoritmo
            using (rijndael)
            {
                //Le pasamos los valores de las claves al algoritmno
                rijndael.Key = pKey;
                rijndael.IV = pIV;

                //Creamos el Encriptador
                System.Security.Cryptography.ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

                //Se crean las corrientes para el cifrado
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (System.Security.Cryptography.CryptoStream csEncrypt = new System.Security.Cryptography.CryptoStream(msEncrypt, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Escribe los datos en la secuencia
                            swEncrypt.Write(pObject);
                        }

                        //Almacenamos la encriptacion en el objecto
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            //Retornamos el objecto encriptado
            return encrypted;
        }

        /// <summary>
        /// Desencripta un objecto
        /// </summary>
        /// <param name="pObject">Objecto Encriptado</param>
        /// <param name="pKey">Clave 1</param>
        /// <param name="pIV">Clave 2</param>
        /// <returns>Retorna el Objecto Desencriptado</returns>
        private static object DecryptObjectFromBytes(byte[] pObject, byte[] pKey, byte[] pIV)
        {
            //Verifica si los parametros son nulo
            if (pObject == null || pObject.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (pKey == null || pKey.Length <= 0)
            {
                throw new ArgumentNullException();
            }
            if (pIV == null || pIV.Length <= 0)
            {
                throw new ArgumentNullException();
            }

            //Variable que almacenara el valor del objecto encritado
            object vlDecodeObject = null;

            //Creamos el algoritmo
            System.Security.Cryptography.Rijndael rijndael = System.Security.Cryptography.Rijndael.Create();

            //Usamos el algoritmo
            using (rijndael)
            {
                //Le pasamos los valores de las claves al algoritmno
                rijndael.Key = pKey;
                rijndael.IV = pIV;

                //Creamos el Desencriptador
                System.Security.Cryptography.ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

                //Se crean las corrientes para el cifrado
                using (MemoryStream msDecrypt = new MemoryStream(pObject))
                {
                    using (System.Security.Cryptography.CryptoStream csDecrypt = new System.Security.Cryptography.CryptoStream(msDecrypt, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            //Escribe los datos en la secuencia, como un objecto (string) 
                            vlDecodeObject = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            //Retornamos el objecto desencriptado
            return vlDecodeObject;
        }

    }
}
