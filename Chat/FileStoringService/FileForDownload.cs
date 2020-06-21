using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStoringService
{
    public class FileForDownload
    {
        public string FileName { get; }
        public byte[] FileBytes { get; }

        public FileForDownload(string fileName, byte[] fileBytes)
        {
            FileName = fileName;
            FileBytes = fileBytes;
        }

    }
}
