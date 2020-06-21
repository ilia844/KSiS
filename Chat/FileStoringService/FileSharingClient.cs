using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace FileStoringService
{
    public delegate void UpdateFilesListDelegate(Dictionary<int, string> filesNameList);

    public class FileSharingClient
    {
        private int MaxFileSize = 3 * 1024 * 1024;
        private const int MaxTotalFilesSize = 10 * 1024 * 1024;
        private static string[] AvailableExtensions = new string[]
        {
            ".txt", ".png", ".jpeg", ".docx"
        };

        public Dictionary<int, string> filesToSend { get; set; }

        public event UpdateFilesListDelegate UpdateFilesListEvent;

        public int totalFilesSize;

        public FileSharingClient()
        {
            filesToSend = new Dictionary<int, string>();
            totalFilesSize = 0;
        }

        public void ActivateUpdateFilesListEvent()
        {
            UpdateFilesListEvent(filesToSend);
        }

        public async Task<FileForDownload> DownloadFile(int fileId, string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage getRequest = new HttpRequestMessage(HttpMethod.Get, url + fileId);

                HttpResponseMessage getResponse = await httpClient.SendAsync(getRequest);

                if (getResponse.IsSuccessStatusCode)
                {
                    FileForDownload fileForDownload = await GetFileByResponse(getResponse);
                    return fileForDownload;
                }
                return null;
            }
        }

        private async Task<FileForDownload> GetFileByResponse(HttpResponseMessage response)
        {
            Stream reader = await response.Content.ReadAsStreamAsync();
            byte[] fileBytes = new byte[reader.Length];
            reader.Read(fileBytes, 0, fileBytes.Length);

            IEnumerable<string> fileNameValues;
            if (response.Headers.TryGetValues("FileName", out fileNameValues))
            {
                string fileName = fileNameValues.First().Substring(5);
                return new FileForDownload(fileName, fileBytes);
            }
            return null;
        }

        public async Task DeleteFile(int fileId, string url)
        {
            using (HttpClient httpCLient = new HttpClient())
            {
                HttpRequestMessage deleteRequest = new HttpRequestMessage(HttpMethod.Delete, url + fileId);

                HttpResponseMessage deleteResponse = await httpCLient.SendAsync(deleteRequest);

                if (deleteResponse.IsSuccessStatusCode)
                {
                    filesToSend.Remove(fileId);
                    ActivateUpdateFilesListEvent();
                }
            }
        }

        public int GetFileIdByFilesToSend(string fileInfo)
        {
            foreach (KeyValuePair<int, string> file in filesToSend)
            {
                if (fileInfo == file.Value)
                {
                    return file.Key;
                }
            }
            return -1;
        }

        public async Task<FileInfo> GetFileInfo(int fileId, string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage headRequest = new HttpRequestMessage(HttpMethod.Head, url + fileId);

                HttpResponseMessage headResponse = await httpClient.SendAsync(headRequest);

                if (headResponse.IsSuccessStatusCode)
                {
                    return GetFileInfoByResponse(headResponse);
                }
                return null;
            }
        }

        private FileInfo GetFileInfoByResponse(HttpResponseMessage response)
        {
            HttpResponseHeaders responseHeaders = response.Headers;
            IEnumerable<string> fileNameValues;
            IEnumerable<string> fileSizeValues;
            if (responseHeaders.TryGetValues("FileName", out fileNameValues) && responseHeaders.TryGetValues("FileSize", out fileSizeValues))
            {
                return new FileInfo(fileNameValues.First().Substring(5), Int32.Parse(fileSizeValues.First()));
            }
            return null;
        }

        public async Task SendFile(string filePath, string url)
        {
            if (IsCorrectFile(filePath))
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string fileName = Path.GetFileName(filePath);
                    fileName = GetUniquePrefix(fileName) + fileName;
                    int fileSize = (int)(new System.IO.FileInfo(filePath).Length);

                    HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, url);
                    postRequest.Headers.Add("FileName", fileName);
                    postRequest.Content = GetMultipartData(filePath);

                    HttpResponseMessage postResponse = await httpClient.SendAsync(postRequest);

                    if (postResponse.IsSuccessStatusCode)
                    {
                        int fileId = GetFileIdFromResponse(postResponse);
                        if (fileId != -1)
                        {
                            FileInfo fileInfo = new FileInfo(fileName, fileSize);
                            filesToSend.Add(fileId, fileInfo.FileName.Substring(5) + " " + GetKilobytesSize(fileInfo.FileSize) + " KB");
                            ActivateUpdateFilesListEvent();
                        }
                    }

                }
            }
        }

        private bool IsCorrectFile(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);

            if (IsAvailableExtension(fileExtension))
            {
                int fileSize = (int)(new System.IO.FileInfo(filePath).Length);
                if (fileSize < MaxFileSize)
                {
                    totalFilesSize += fileSize;
                    if (totalFilesSize < MaxTotalFilesSize)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        private bool IsAvailableExtension(string fileExtension)
        {
            foreach (string availableExtension in AvailableExtensions)
            {
                if (fileExtension == availableExtension)
                {
                    return true;
                }
            }
            return false;
        }

        private string GetUniquePrefix(string fileName)
        {
            int seconds = DateTime.Now.Second;
            Random random = new Random();
            int randomValue = random.Next(100000, 99999999);

            string uniquePrefix = (seconds + randomValue).ToString();
            return uniquePrefix.Substring(uniquePrefix.Length - 5);
        }

        private MultipartFormDataContent GetMultipartData(string filePath)
        {
            MultipartFormDataContent multipartFormData = new MultipartFormDataContent();
            ByteArrayContent fileBytesContent = GetFileBytesFromFile(filePath);
            multipartFormData.Add(fileBytesContent);
            return multipartFormData;
        }

        private ByteArrayContent GetFileBytesFromFile(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                byte[] fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, fileBytes.Length);
                return new ByteArrayContent(fileBytes);
            }
        }

        private int GetFileIdFromResponse(HttpResponseMessage response)
        {
            HttpResponseHeaders responseHeaders = response.Headers;
            IEnumerable<string> fileIdValues;
            if (responseHeaders.TryGetValues("FileId", out fileIdValues))
            {
                return Int32.Parse(fileIdValues.First());
            }
            return -1;
        }

        private string GetKilobytesSize(int bytes)
        {
            return string.Format("{0:F2}", ((double)bytes / 1024));
        }
    }
}
