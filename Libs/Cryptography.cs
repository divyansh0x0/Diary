using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace LifeInDiary.Libs
{
    public static class Cryptography
    {
        private static string Key = "@&trg2-$di8v-##cght22";
        /// <summary>
        /// Returns an base 64 encrypted string from the given TextRange 
        /// </summary>
        /// <param name="TextRangeToEncrypt">The text range to encrypt</param>
        /// <returns></returns>
        #region Text Range
        public static string EncryptTextRange(TextRange TextRangeToEncrypt)
        {
            MemoryStream memoryStream = new MemoryStream();
            TextRangeToEncrypt.Save(memoryStream, DataFormats.Rtf);

            string passPhrase = Key;
            string saltValue = "sALtValue";
            string hashAlgorithm = "SHA1";
            int passwordIterations = 7;
            string initVector = "~1B2c3D4e5F6g7H8";
            int keySize = 256;

            byte[] bytes = Encoding.ASCII.GetBytes(initVector);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
            byte[] buffer = memoryStream.GetBuffer();
            byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
            RijndaelManaged managed = new RijndaelManaged();
            managed.Mode = CipherMode.CBC;
            ICryptoTransform transform = managed.CreateEncryptor(rgbKey, bytes);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            byte[] inArray = stream.ToArray();
            stream.Close();
            stream2.Close();
            stream.Dispose();
            stream2.Dispose();
            return Convert.ToBase64String(inArray);
        }
        #endregion
        #region
        public static string EncryptString(string data)
        {
            string passPhrase = Key;
            string saltValue = "sALtValue";
            string hashAlgorithm = "SHA1";
            int passwordIterations = 7;
            string initVector = "~1B2c3D4e5F6g7H8";
            int keySize = 256;

            byte[] bytes = Encoding.ASCII.GetBytes(initVector);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
            RijndaelManaged managed = new RijndaelManaged();
            managed.Mode = CipherMode.CBC;
            ICryptoTransform transform = managed.CreateEncryptor(rgbKey, bytes);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            byte[] inArray = stream.ToArray();
            stream.Close();
            stream2.Close();
            stream.Dispose();
            stream2.Dispose();
            return Convert.ToBase64String(inArray);
        }
        /// <summary>
        /// Returns utf 8 string of after decoding the data
        /// </summary>
        /// <param name="data">String to decode</param>
        /// <returns></returns>
        public static string DecryptString(string data)
        {
            string passPhrase = Key;
            string saltValue = "sALtValue";
            string hashAlgorithm = "SHA1";
            int passwordIterations = 7;
            string initVector = "~1B2c3D4e5F6g7H8";
            int keySize = 256;

            byte[] bytes = Encoding.ASCII.GetBytes(initVector);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(saltValue);
            byte[] buffer = Convert.FromBase64String(data);
            byte[] rgbKey = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
            RijndaelManaged managed = new RijndaelManaged();
            managed.Mode = CipherMode.CBC;
            ICryptoTransform transform = managed.CreateDecryptor(rgbKey, bytes);
            MemoryStream stream = new MemoryStream(buffer);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            byte[] buffer5 = new byte[buffer.Length];
            int count = stream2.Read(buffer5, 0, buffer5.Length);
            stream.Close();
            stream2.Close();
            stream.Dispose();
            stream2.Dispose();
            return Encoding.UTF8.GetString(buffer5, 0, count);
        }
        #endregion
    }
}
