using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace FileStoringService
{
    public class FileStoringManager
    {
        private static string fileStorageDirectoryPath = Directory.GetCurrentDirectory() + "\\FileRepository\\";
        private const int startFileId = 0;

        private Dictionary<int, string> files;

        public FileStoringManager()
        {
            files = new Dictionary<int, string>();
            CreateOrClearFileRepository();
        }

        private void CreateOrClearFileRepository()
        {
            if (!(Directory.Exists(fileStorageDirectoryPath)))
            {
                Directory.CreateDirectory(fileStorageDirectoryPath);
            }
            else
            {
                var fileStorageDirectory = new DirectoryInfo(fileStorageDirectoryPath);
                foreach (var file in fileStorageDirectory.GetFiles())
                {
                    file.Delete();
                }
            }

        }

        private int GetNewId()
        {
            if (files.Count == 0 )
            {
                return startFileId;
            }
            else
            {
                return files.Keys.Last() + 1;
            }
        }

        public bool AddNewFile(string fileName, byte[] fileBytes, ref int id)
        {
            if (!IsFileExists(fileName))
            {
                string downloadPath = fileStorageDirectoryPath + fileName;
                if (WriteFileToFileRepository(downloadPath, fileBytes))
                {
                    id = GetNewId();
                    files.Add(id, fileName);
                    return true;
                }
            }
            return false;
        }

        private bool WriteFileToFileRepository(string downloadPath, byte[] fileBytes)
        {
            try
            {
                using (FileStream fileStream = new FileStream(downloadPath, FileMode.Create))
                {
                    fileStream.Write(fileBytes, 0, fileBytes.Length);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteFile(int fileId)
        {
            if (IsFileExists(fileId))
            {
                string filePath = fileStorageDirectoryPath + GetFileNameById(fileId);
                File.Delete(filePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetFileNameById(int fileId)
        {
            foreach (KeyValuePair<int, string> file in files)
            {
                if (file.Key == fileId)
                {
                    return file.Value;
                }
            }
            return null;
        } 

        public bool GetFileInfo(int fileId, ref string fileName, ref int fileSize)
        {
            if (IsFileExists(fileId))
            {
                fileName = GetFileNameById(fileId);
                string filePath = fileStorageDirectoryPath + fileName;
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {  
                    fileSize = (int)fileStream.Length;
                }
                return true;
            }
            return false;
        }

        public byte[] GetBytesForDownload(int fileId)
        {
            if (IsFileExists(fileId))
            {
                string filePath = fileStorageDirectoryPath + GetFileNameById(fileId);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    byte[] fileBytes = new byte[fileStream.Length];
                    fileStream.Read(fileBytes, 0, fileBytes.Length);
                    return fileBytes;
                }
            }
            return null;                
        }

        public bool IsFileExists(string fileName)
        {
            foreach (KeyValuePair<int, string> file in files)
            {
                if ( file.Value == fileName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsFileExists(int fileId)
        {
            foreach (KeyValuePair<int, string> file in files)
            {
                if ( file.Key == fileId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
