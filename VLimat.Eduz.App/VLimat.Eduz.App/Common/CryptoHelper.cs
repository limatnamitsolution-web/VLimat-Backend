using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VLimat.Eduz.App.Common
{
    public static class CryptoHelper
    {
        public static string DecryptString(string cipherText, string key)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                throw new ArgumentNullException(nameof(cipherText));

            // Accept URL-encoded and URL-safe Base64
            string base64 = Uri.UnescapeDataString(cipherText).Trim();
            base64 = base64.Replace('-', '+').Replace('_', '/');

            // Pad to multiple of 4
            int mod4 = base64.Length % 4;
            if (mod4 == 1)
                throw new FormatException("Invalid base64 string length.");
            if (mod4 > 0)
                base64 = base64.PadRight(base64.Length + (4 - mod4), '=');

            byte[] buffer;
            try
            {
                buffer = Convert.FromBase64String(base64);
            }
            catch (FormatException ex)
            {
                throw new FormatException("The input is not a valid Base-64 string.", ex);
            }

            // If OpenSSL salted format (used by CryptoJS when passing a passphrase)
            // format: "Salted__" (8 bytes) + 8 bytes salt + ciphertext
            byte[] keyBytes;
            byte[] iv = new byte[16];
            byte[] cipherBytes;

            if (buffer.Length > 16 && IsOpenSslSalted(buffer))
            {
                // Extract salt and ciphertext
                byte[] salt = new byte[8];
                Array.Copy(buffer, 8, salt, 0, 8);
                cipherBytes = new byte[buffer.Length - 16];
                Array.Copy(buffer, 16, cipherBytes, 0, cipherBytes.Length);

                // Derive key and IV using OpenSSL EVP_BytesToKey (MD5)
                EvpBytesToKey(Encoding.UTF8.GetBytes(key), salt, 32, 16, out keyBytes, out iv);
            }
            else
            {
                // Prepare key bytes:
                keyBytes = Encoding.UTF8.GetBytes(key);
                // Keep raw key if valid AES length; otherwise derive a 32-byte key with SHA-256.
                if (!(keyBytes.Length == 16 || keyBytes.Length == 24 || keyBytes.Length == 32))
                {
                    keyBytes = SHA256.HashData(Encoding.UTF8.GetBytes(key));
                }

                // Determine IV and ciphertext:
                if (buffer.Length > 16)
                {
                    // treat first 16 bytes as IV (common pattern when encryption used a random IV)
                    Array.Copy(buffer, 0, iv, 0, 16);
                    cipherBytes = new byte[buffer.Length - 16];
                    Array.Copy(buffer, 16, cipherBytes, 0, cipherBytes.Length);
                }
                else if (buffer.Length == 16)
                {
                    // Only IV present -> nothing to decrypt
                    throw new CryptographicException("Ciphertext contains only an IV and no encrypted payload.");
                }
                else
                {
                    // No IV prefixed; use zero IV (preserves previous behavior if the encryptor used zero IV)
                    cipherBytes = buffer;
                }
            }

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (MemoryStream ms = new MemoryStream(cipherBytes))
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (StreamReader reader = new StreamReader(cs, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (CryptographicException ex)
            {
                // Provide a clearer message about possible causes
                throw new CryptographicException("Decryption failed. Possible causes: wrong key, wrong IV, corrupted ciphertext, or mismatched encryption parameters.", ex);
            }
        }

        private static bool IsOpenSslSalted(byte[] data)
        {
            if (data.Length < 16) return false;
            // "Salted__" ASCII bytes
            return data[0] == (byte)'S' &&
                   data[1] == (byte)'a' &&
                   data[2] == (byte)'l' &&
                   data[3] == (byte)'t' &&
                   data[4] == (byte)'e' &&
                   data[5] == (byte)'d' &&
                   data[6] == (byte)'_' &&
                   data[7] == (byte)'_';
        }

        // Implements OpenSSL EVP_BytesToKey using MD5 (the same derivation CryptoJS uses for passphrase-based encryption)
        private static void EvpBytesToKey(byte[] password, byte[] salt, int keyLen, int ivLen, out byte[] key, out byte[] iv)
        {
            List<byte> derived = new List<byte>();
            byte[] block = Array.Empty<byte>();

            using (var md5 = MD5.Create())
            {
                while (derived.Count < (keyLen + ivLen))
                {
                    // D_i = MD5(D_{i-1} || password || salt)
                    byte[] data = new byte[block.Length + password.Length + (salt?.Length ?? 0)];
                    Buffer.BlockCopy(block, 0, data, 0, block.Length);
                    Buffer.BlockCopy(password, 0, data, block.Length, password.Length);
                    if (salt != null)
                        Buffer.BlockCopy(salt, 0, data, block.Length + password.Length, salt.Length);

                    block = md5.ComputeHash(data);
                    derived.AddRange(block);
                }
            }

            byte[] derivedBytes = derived.ToArray();
            key = new byte[keyLen];
            iv = new byte[ivLen];
            Array.Copy(derivedBytes, 0, key, 0, keyLen);
            Array.Copy(derivedBytes, keyLen, iv, 0, ivLen);
        }
    }
}
