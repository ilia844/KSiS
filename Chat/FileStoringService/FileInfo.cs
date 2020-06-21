using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStoringService
{
    public class FileInfo
    {
        public string FileName { get; }
        public int FileSize { get; }

        public FileInfo(string fileName, int fileSize) 
        {
            FileName = fileName;
            FileSize = fileSize;
        }

    }
}
