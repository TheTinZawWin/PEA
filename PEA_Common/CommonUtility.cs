using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PEA_Common
{
   public class CommonUtility
    {
        public static string DecryptString(string sourceString, string password)
        {
            try
            {
                //Create a RijndaelManaged object
                RijndaelManaged rijndael = new RijndaelManaged();

                // Create shared key and initialization vector from password
                byte[] key = null;
                byte[] iv = null;

                CommonUtility.GenerateKeyFromPassword(password, rijndael.KeySize, ref key, rijndael.BlockSize, ref iv);
                rijndael.Key = key;
                rijndael.IV = iv;

                //Return a string to a byte array
                byte[] strBytes = Convert.FromBase64String(sourceString);

                // Creating a symmetric encrypted object
                ICryptoTransform decryptor = rijndael.CreateDecryptor();

                // Decrypting a byte array An exception CryptographicException occurs if decoding fails
                byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);

                // close up
                decryptor.Dispose();

                // Returns a byte array back to a string
                return System.Text.Encoding.UTF8.GetString(decBytes);


            }
            catch (FormatException)
            {
                return sourceString;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GenerateKeyFromPassword(string password, int keySize, ref byte[] key, int blockSize, ref byte[] iv)
        {
            // Create a shared key and initialization vector from the password
            byte[] salt = Encoding.UTF8.GetBytes("must be 8 bytes or more");

            //Create an Rfc2898DeriveBytes object
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt);

            //Specify the number of iterations 1000 times by default
            deriveBytes.IterationCount = 1000;

            //Generate shared key and initialization vector
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }

        public static string EncryptString(string sourceString, string password)
        {
            // Create rijndaelManaged object
            var rijndael = new RijndaelManaged();

            // Create shared key and initialization vector from password
            byte[] key = null;
            byte[] iv = null;

            CommonUtility.GenerateKeyFromPassword(password, rijndael.KeySize, ref key, rijndael.BlockSize, ref iv);
            rijndael.Key = key;
            rijndael.IV = iv;

            // Convert a string to a byte array
            byte[] strBytes = Encoding.UTF8.GetBytes(sourceString);

            // Creating a symmetric encrypted object
            ICryptoTransform encryptor = rijndael.CreateEncryptor();

            // Encrypt byte array
            byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);

            // close up
            encryptor.Dispose();

            // Converts a byte array to a string and returns it
            return Convert.ToBase64String(encBytes);
        }
        public static string GenerateHash(string saltKey, string src)
        {
            const int STRETCHING_TIMES = 10000;

           
            string salt = GenerateSalt(saltKey);

           
            var pbkdf2 = new Rfc2898DeriveBytes(src, Encoding.UTF8.GetBytes(salt), STRETCHING_TIMES);

            
            string result = Convert.ToBase64String(pbkdf2.GetBytes(32));

            return result;
        }
        private static string GenerateSalt(string src)
        {
            var data = Encoding.UTF8.GetBytes(src);
            var sha256 = new SHA256CryptoServiceProvider();
            var hash = sha256.ComputeHash(data);
            string result = BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
            return result;
        }
        public class ViewDataParam
        {
          
            public const string Title = "Title";

          
            public const string ScreenId = "ScreenId";

          
            public const string ErrorMessage = "ErrorMessage";
        }
    }
}
