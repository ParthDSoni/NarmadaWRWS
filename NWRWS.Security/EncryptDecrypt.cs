using System.Security.Cryptography;
using System.Text;

namespace NWRWS.Security
{
    public class EncDecFront
    {

        private static string EncryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        public static string DecryptStringAES(string cipherText)
        {
            //var keybytes = Encoding.UTF8.GetBytes("4080808080808020");
            //var iv = Encoding.UTF8.GetBytes("4080808080808020");

            var keybytes = Encoding.UTF8.GetBytes("4090909090909020");
            var iv = Encoding.UTF8.GetBytes("4090909090909020");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }


        public static string EncryptStringAES(string cipherText)
        {
            //var keybytes = Encoding.UTF8.GetBytes("4080808080808020");
            //var iv = Encoding.UTF8.GetBytes("4080808080808020");

            var keybytes = Encoding.UTF8.GetBytes("4090909090909020");
            var iv = Encoding.UTF8.GetBytes("4090909090909020");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = EncryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
    }

    public class EncryptDecrypt
    {

        private static byte[] Key = Encoding.UTF8.GetBytes("tu89geji340t89u2");
        private static byte[] IV = Encoding.UTF8.GetBytes("tu89geji340t89u2");

        public static byte[] EncryptToBytesUsingCBC(string toEncrypt)
        {
            byte[] src = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] dest = new byte[src.Length];
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = 128;
                aes.KeySize = 128;
                aes.IV = IV;
                aes.Key = Key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                // encryption
                using (ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    return encrypt.TransformFinalBlock(src, 0, src.Length);
                }
            }
        }

        public static string EncryptUsingCBC(string toEncrypt)
        {
            return Convert.ToBase64String(EncryptToBytesUsingCBC(toEncrypt));
        }

        public static string DecryptToBytesUsingCBC(byte[] toDecrypt)
        {
            byte[] src = toDecrypt;
            byte[] dest = new byte[src.Length];
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = 128;
                aes.KeySize = 128;
                aes.IV = IV;
                aes.Key = Key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                // decryption
                using (ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decryptedText = decrypt.TransformFinalBlock(src, 0, src.Length);

                    return Encoding.UTF8.GetString(decryptedText);
                }
            }
        }

        public static string DecryptUsingCBC(string toDecrypt)
        {
            string s = DecryptToBytesUsingCBC(Convert.FromBase64String(toDecrypt));
            return s;
        }
    }

}