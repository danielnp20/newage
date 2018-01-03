using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// AESCrypto. Gets the key from the web.config and the initialization vector hard-coded
    /// The crypto application for a learning system does not require high security (does not handle monetary transactions), 
    /// therefore it is kept as simple as possible.
    /// </summary>
    public static class AESCrypto
    {
        private static byte[] _oesCryptoKey = GetCryptoKey();
        private static byte[] _oesInitializationVector = Convert.FromBase64String("XombgbRICv3g3gWcq0D27w==");

        public static bool CheckPassword(string plainText, byte[] encryptedText) {
            byte[] enc = Encrypt(plainText);
            if (enc.Length != encryptedText.Length)
                return false;
            for (int i = 0; i < enc.Length; i++)
                if (encryptedText[i] != enc[i])
                    return false;
            return true;
        }
        
        public static byte[] Encrypt(string plainText)
        {
            RijndaelManaged cpt = new RijndaelManaged();
            cpt.Key = _oesCryptoKey;
            cpt.IV = _oesInitializationVector;
            return encryptStringToBytes_AES(plainText, cpt.Key, cpt.IV);
        }

        public static string Decrypt(byte[] cipherText)
        {
            RijndaelManaged cpt = new RijndaelManaged();
            cpt.Key = _oesCryptoKey;
            cpt.IV = _oesInitializationVector;
            return DecryptStringFromBytes_AES(cipherText, cpt.Key, cpt.IV);
        }

        private static byte[] encryptStringToBytes_AES(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null)
                throw new ArgumentNullException("plainText"); // allow empty plain text
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("Key");

            MemoryStream msEncrypt = null; // stream used to encrypt to an in memory array of bytes.
            RijndaelManaged aesAlg = null; // object used to encrypt the data.

            try
            {
                // Create a RijndaelManaged object with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return msEncrypt.ToArray();
        }

        private static string DecryptStringFromBytes_AES(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            RijndaelManaged aesAlg = null; // object used to decrypt the data.

            string plaintext = null;

            try
            {
                // Create a RijndaelManaged object with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return plaintext;
        }

        //TODO SOLVE THIS
        private static byte[] GetCryptoKey()
        {
            try
            {
                byte[] bs = Convert.FromBase64String(System.Configuration.ConfigurationManager.AppSettings["CryptoKey"]);
                
                if (bs == null || bs.Length <= 0)
                    throw new ApplicationException("CryptoKey is empty or error getting it.");
                return bs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to read key from web.config.", ex);
            }
        }
    }
}
