using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Secure.Model
{
    public class AES
    {
        public enum Process
        {
            Encryption,
            Decryptopn
        }

        private readonly AesCryptoServiceProvider aes;

        public AES()
        {
            this.aes = new AesCryptoServiceProvider();
            this.aes.KeySize = 256;
            this.aes.BlockSize = 128;
        }

        public byte[] Encrypt(byte[] plain, string password, Process process = Process.Encryption)
        {
            try
            {
                using (var pass = new PasswordDeriveBytes(password, this.GenerateSalt(this.aes.BlockSize / 8, password)))
                {
                    using (var stream = new MemoryStream())
                    {
                        this.aes.Key = pass.GetBytes(this.aes.KeySize / 8);
                        this.aes.IV = pass.GetBytes(this.aes.BlockSize / 8);

                        var proc = (process == Process.Encryption) ? this.aes.CreateEncryptor() : this.aes.CreateDecryptor();
                        using (var crypto = new CryptoStream(stream, proc, CryptoStreamMode.Write))
                        {
                            crypto.Write(plain, 0, plain.Length);
                            crypto.Clear();
                            crypto.Close();
                        }
                        stream.Close();
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public byte[] Decrypt(byte[] encrypted, string password)
        {
            return this.Encrypt(encrypted, password, Process.Decryptopn);
        }

        private byte[] GenerateSalt(int size, string password)
        {
            var buffer = new byte[size];
            var passBytes = ASCIIEncoding.ASCII.GetBytes(password);

            if (passBytes.Length > buffer.Length) Array.Copy(passBytes, buffer, buffer.Length);
            else Array.Copy(passBytes, buffer, passBytes.Length);

            return buffer;
        }
    }
}
