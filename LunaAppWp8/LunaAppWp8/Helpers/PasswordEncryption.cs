using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LunaAppWp8.Helpers
{
    public static class PasswordEncryption
    {
        private const string FILE_DIR = "DataFolder";
        private const string FILE_PATH = "DataFolder\\Pwd.txt";

        private static void WritePinToFile(byte[] pinData)
        {
            IsolatedStorageFile local = IsolatedStorageFile.GetUserStoreForApplication();

            if (!local.DirectoryExists(FILE_DIR))
                local.CreateDirectory(FILE_DIR);

            // Create a file in the application's isolated storage.
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream writestream = new IsolatedStorageFileStream(FILE_PATH, System.IO.FileMode.Create, System.IO.FileAccess.Write, file);

            // Write pinData to the file.
            Stream writer = new StreamWriter(writestream).BaseStream;
            writer.Write(pinData, 0, pinData.Length);
            writer.Close();
            writestream.Close();
        }

        private static byte[] ReadPinFromFile()
        {
            // Access the file in the application's isolated storage.
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream readstream = new IsolatedStorageFileStream(FILE_PATH, System.IO.FileMode.Open, FileAccess.Read, file);

            // Read the PIN from the file.
            Stream reader = new StreamReader(readstream).BaseStream;
            byte[] pinArray = new byte[reader.Length];

            reader.Read(pinArray, 0, pinArray.Length);
            reader.Close();
            readstream.Close();

            return pinArray;
        }

        public static void StorePassword( string pwd)
        {
            byte[] pinByte = Encoding.UTF8.GetBytes(pwd);

            byte[] protectedPinByte = ProtectedData.Protect(pinByte, null);
            WritePinToFile(protectedPinByte);
        }

        public static string RetrievePassword()
        {
            byte[] protectedPinBytes = ReadPinFromFile();

            // Decrypt the PIN by using the Unprotect method.
            byte[] pinBytes = ProtectedData.Unprotect(protectedPinBytes, null);

            string pwd = Encoding.UTF8.GetString(pinBytes, 0, pinBytes.Count());

            return pwd;
        }
    }
}
