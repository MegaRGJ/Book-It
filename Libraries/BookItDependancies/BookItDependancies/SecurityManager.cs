using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookItDependancies
{
    class SecurityManager
    {
       //TODO store encrypted version of encryption pass in file handler (Needs to be seperate from this file

        /// <summary>
        /// Encrypts a string with the given SALT
        /// </summary>
        /// <param name="toPass"></param>
        /// <param name="SALT"></param>
        /// <returns></returns>
        public string OneWayEncryptor(string toPass, byte[] SALT)
        {
            toPass += Encoding.Default.GetString(SALT);      //Adds SALT to string to be passed

            byte[] data = new byte[toPass.Length + SALT.Length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(toPass[i]);
            }

            byte[] encryptResult;

            SHA256 shaM = new SHA256Managed();
            encryptResult = shaM.ComputeHash(data);

            return Convert.ToString(encryptResult); //Returns encrypted data
        }

        public string SecretKeyEncryptor(string toPass, string encryptionPass = "T%%w54Pjf6$$d")
        {
            byte[] salt = GenerateNewSALT(8);
            string saltString = Encoding.Default.GetString(salt);

            string encryption = AESTwoWayEncryption.Encrypt<TripleDESCryptoServiceProvider>(toPass, encryptionPass, saltString);

            return SetEncryptString(encryption, saltString);
        }

        public string SecretKeyDecryptor(string toPass, string encryptionPass = "T%%w54Pjf6$$d")
        {
            string saltString = GetEncryptString(toPass);
            string decryption = AESTwoWayEncryption.Decrypt<TripleDESCryptoServiceProvider>(GetStringToDecrypt(toPass), encryptionPass, saltString);
            return decryption;
        }

        /// <summary>
        /// Generates a new SALT byte array
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateNewSALT(int len = 16)
        {
            byte[] salt = new byte[len];
            using (var ran = new RNGCryptoServiceProvider()) { ran.GetNonZeroBytes(salt); }
            return salt;
        }

        private string SetEncryptString(string encrypt, string salt)
        {
            string salt1 = salt.Substring(0, 4);
            string salt2 = salt.Substring(4, 4);

            return salt1 + encrypt + salt2;
        }
        private string GetEncryptString(string encrypt) {
            return encrypt.Substring(0, 4) + encrypt.Substring(encrypt.Length - 5, 4);
        }
        private string GetStringToDecrypt(string decrypt) {
            string s = decrypt.Remove(0, 4);
            return s.Remove(s.Length - 5, 4);
        }
    }

    public class AESTwoWayEncryption
    {
        public static string Encrypt<T>(string value, string password, string salt)
        where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }

                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        public static string Decrypt<T>(string text, string password, string salt)
           where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(text)))
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
