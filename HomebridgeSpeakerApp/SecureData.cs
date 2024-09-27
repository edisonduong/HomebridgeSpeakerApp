using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HomebridgeSpeakerApp
{
    public static class SecureData
    {
        private static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HomebridgeSpeakerApp", "config.dat");
        private static readonly byte[] entropy = Encoding.UTF8.GetBytes("your-random-entroy-key");

        public static void SaveCredentials(string homebridgeIP, string homebridgePort, string username, string password, string accessoryID)
        {
            string data = $"{homebridgeIP}|{homebridgePort}|{username}|{password}|{accessoryID}";
            byte[] encryptedData = ProtectData(data);
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            File.WriteAllBytes(FilePath, encryptedData);
        }

        public static (string homebridgeIP, string homebridgePort, string username, string password, string accessoryID)? LoadCredentials()
        {
            if (!File.Exists(FilePath))
                return null;

            try
            {
                byte[] encryptedData = File.ReadAllBytes(FilePath);
                string decryptedData = UnprotectData(encryptedData);
                var parts = decryptedData.Split('|');
                if (parts.Length == 5)
                {
                    return (parts[0], parts[1], parts[2], parts[3], parts[4]);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }
        public static void DeleteCredentials()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

        private static byte[] ProtectData(string data)
        {
            return ProtectedData.Protect(Encoding.UTF8.GetBytes(data), entropy, DataProtectionScope.CurrentUser);
        }

        private static string UnprotectData(byte[] data)
        {
            return Encoding.UTF8.GetString(ProtectedData.Unprotect(data, entropy, DataProtectionScope.CurrentUser));
        }
    }
}
