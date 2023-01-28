using System;
using System.IO;
using System.Text;

namespace VGLabsFoundation
{
    public static class Encryption
    {

        // Get File Hash using SHA1
        public static string GetSHA1FileHash(string filePath)
        {
            try
            {
                using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
                {
                    byte[] hash;
                    using (var stream = File.OpenRead(filePath))
                    {
                        hash = sha1.ComputeHash(stream);
                    }
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating SHA1 file hash for " + filePath);
                return null;
            }
        }

        // Get String Hash using SHA1
        public static string GetSHA1Hash(string input)
        {
            try
            {
                using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
                {
                    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating SHA1 string hash of " + input);
                return null;
            }
        }

        // Get File Hash using SHA256
        public static string GetSHA256FileHash(string filePath)
        {
            try
            {
                using (var sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider())
                {
                    byte[] hash;
                    using (var stream = File.OpenRead(filePath))
                    {
                        hash = sha256.ComputeHash(stream);
                    }
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating SHA256 file hash for " + filePath);
                return null;
            }
        }

        // Get String Hash using SHA256
        public static string GetSHA256Hash(string input)
        {
            try
            {
                using (var sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider())
                {
                    var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating SHA256 string hash of " + input);
                return null;
            }
        }

        // Get File Hash using SHA384
        public static string GetSHA384FileHash(string filePath)
        {
            try
            {
                using (var sha384 = new System.Security.Cryptography.SHA384CryptoServiceProvider())
                {
                    byte[] hash;
                    using (var stream = File.OpenRead(filePath))
                    {
                        hash = sha384.ComputeHash(stream);
                    }
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating SHA384 file hash for " + filePath);
                return null;
            }
        }

        // Get String Hash using SHA384
        public static string GetSHA384Hash(string input)
        {
            try
            {
                using (var sha384 = new System.Security.Cryptography.SHA384CryptoServiceProvider())
                {
                    var hash = sha384.ComputeHash(Encoding.UTF8.GetBytes(input));
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating SHA384 string hash of " + input);
                return null;
            }
        }

        // Get File Hash using SHA512
        public static string GetSHA512FileHash(string filePath)
        {
            try
            {
                using (var sha512 = new System.Security.Cryptography.SHA512CryptoServiceProvider())
                {
                    byte[] hash;
                    using (var stream = File.OpenRead(filePath))
                    {
                        hash = sha512.ComputeHash(stream);
                    }
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating SHA512 file hash for " + filePath);
                return null;
            }
        }

        // Get String Hash using SHA512
        public static string GetSHA512Hash(string input)
        {
            try
            {
                using (var sha512 = new System.Security.Cryptography.SHA512CryptoServiceProvider())
                {
                    var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating SHA512 string hash of " + input);
                return null;
            }
        }

        // Get File Hash using MD5
        public static string GetMD5FileHash(string filePath)
        {
            try
            {
                using (var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    byte[] hash;
                    using (var stream = File.OpenRead(filePath))
                    {
                        hash = md5.ComputeHash(stream);
                    }
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating MD5 file hash for " + filePath);
                return null;
            }
        }

        // Get String Hash using MD5
        public static string GetMD5Hash(string input)
        {
            try
            {
                using (var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating MD5 string hash of " + input);
                return null;
            }
        }

        // Get File Checksum
        public static string[] GetFileChecksum(string filePath)
        {
            try
            {
                if (filePath != null)
                {
                    if (Extraction.ExtractFileExists(filePath))
                    {
                        string[] checksum = {
                            GetMD5FileHash(filePath),
                            GetSHA1FileHash(filePath),
                            GetSHA256FileHash(filePath),
                            GetSHA384FileHash(filePath),
                            GetSHA512FileHash(filePath),
                        };
                        return checksum;
                    }
                }

                return null;
            }
            catch (Exceptions ex)
            {
                ex.LogError("Error generating file hash checksum of " + filePath);
                return null;
            }
        }

    }
}


