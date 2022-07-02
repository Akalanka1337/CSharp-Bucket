using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cyberscap
{
    class SecureStrings //@Akalanka1337 -7/2/22
    {
        /* Note: 
         * 1.First of all check database length and SP param lengths are compatible for long strings! */

        String BasicKey = "KEY_HERE"; //(32Bit)

        public string ProtectString(string key, string secretString)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(On128BitBasedStr(key));
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(secretString);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string RestoreString(string key, string hiddenString)
        {
            if (key == "ZZZ")
            {
                key = BasicKey;
            }
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(hiddenString);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(On128BitBasedStr(key));
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string On128BitBasedStr(string keygen)
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                b.Append(keygen[i % keygen.Length]);
            }
            keygen = b.ToString();

            return keygen;
        }
    }
}
