using IttsabusAPI.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace IttsabusAPI.EndPoint.Clases
{
    public static class FuncionesComunes
    {
        public static bool IsBase64String(this string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        public static string Decrypt(string cipherText)
        {

            string EncryptionKey = "ENC_KEY";// "MAKV2SPBNI99212";
                                             //cipherText = cipherText.Replace(" ", "+");

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Padding = PaddingMode.PKCS7;
                byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, SALT);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Flush();
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string Encrypt(string plainText)
        {
            string EncryptionKey = "ENC_KEY"; // Mismo valor que en Decrypt
            byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

            byte[] clearBytes = Encoding.Unicode.GetBytes(plainText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Padding = PaddingMode.PKCS7;
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, SALT);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

                
        public static bool IsDate(object Expression)
        {
            if (Expression != null)
            {
                if (Expression is DateTime)
                {
                    return true;
                }
                if (Expression is string)
                {
                    DateTime time1;
                    return DateTime.TryParse((string)Expression, out time1);
                }
            }
            return false;
        }

        public static Boolean IsNumber(String s)
        {
            Boolean value = true;
            if (s == String.Empty || s == null)
            {
                value = false;
            }
            else
            {
                foreach (Char c in s.ToCharArray())
                {
                    value = value && Char.IsDigit(c);
                }
            }
            return value;
        }
    }
}