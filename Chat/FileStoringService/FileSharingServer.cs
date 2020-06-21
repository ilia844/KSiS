using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace FileStoringService
{
    public class FileSharingServer
    {
        private FileStoringManager fileStoringManager;
        private HttpListener httpListener;
        private Thread threadListen;
        private bool isListen;

        public FileSharingServer(string serverUrl)
        {
            fileStoringManager = new FileStoringManager();
            httpListener = new HttpListener();
            isListen = false;
            threadListen = new Thread(Listen);
            httpListener.Prefixes.Add(serverUrl); 
        }

        public void Start()
        {
            threadListen.Start();
        }

        public void Listen()
        {
            try
            {
                httpListener.Start();
                isListen = true;
                while (isListen)
                {
                    HttpListenerContext context = httpListener.GetContext();
                    Task task = Task.Run(() => HandleContext(ref context));
                }
            }
            catch 
            {
                isListen = false;
            }
        }
        
        private void HandleContext(ref HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            if (request.HttpMethod == "GET")
            {
                HandleGetRequest(request, ref response);
            }

            if (request.HttpMethod == "POST")
            {
                HandlePostRequest(request, ref response);
            }

            if (request.HttpMethod == "HEAD")
            {
                HandleHeadRequest(request, ref response);
            }

            if (request.HttpMethod == "DELETE")
            {
                HandleDeleteRequest(request, ref response);
            }
        }

        private void HandleGetRequest(HttpListenerRequest request, ref HttpListenerResponse response)
        {
            int fileId = Int32.Parse(request.Url.LocalPath.Substring(1));
            string fileName = fileStoringManager.GetFileNameById(fileId);

            if (fileStoringManager.IsFileExists(fileId) && fileName != null)
            {
                response.AddHeader("FileName", fileName);

                byte[] bytesForDownload = fileStoringManager.GetBytesForDownload(fileId);
                using (Stream outputStream = response.OutputStream)
                {
                    outputStream.Write(bytesForDownload, 0, bytesForDownload.Length);
                }

                response.StatusCode = 200;
                response.StatusDescription = "OK";
            }
            else
            {
                response.StatusCode = 200;
                response.StatusDescription = "File with such id not found";
            }

            response.OutputStream.Close();
        }

        private byte[] GetFileBytesFromRequest(HttpListenerRequest request)
        {
            StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding);
            string resultString = reader.ReadToEnd();
            return request.ContentEncoding.GetBytes(resultString);
        }

        private void HandlePostRequest(HttpListenerRequest request, ref HttpListenerResponse response)
        {
            string fileName = request.Headers.Get("FileName");
            byte[] fileBytes = GetFileBytesFromRequest(request);
            int fileId = 0;

            if (fileStoringManager.AddNewFile(fileName, fileBytes, ref fileId))
            {
                response.StatusCode = 200;
                response.StatusDescription = "OK";
                response.Headers.Add("FileId", fileId.ToString());
            }
            else
            {
                response.StatusCode = 409;
                response.StatusDescription = "File already exists on the server!";
            }

            response.OutputStream.Close();
        }

        private void HandleHeadRequest(HttpListenerRequest request, ref HttpListenerResponse response)
        {
            int fileId = Int32.Parse(request.Url.LocalPath.Substring(1));
            string fileName = "";
            int fileSize = 0;

            if (fileStoringManager.GetFileInfo(fileId, ref fileName, ref fileSize))
            {
                response.Headers.Add("FileName", fileName);
                response.Headers.Add("FileSize", fileSize.ToString());

                response.StatusCode = 200;
                response.StatusDescription = "OK";
            }
            else
            {
                response.StatusCode = 404;
                response.StatusDescription = "File with such id not found!";
            }

            response.OutputStream.Close();
        }
        
        private void HandleDeleteRequest(HttpListenerRequest request, ref HttpListenerResponse response)
        {
            int fileId = Int32.Parse(request.Url.LocalPath.Substring(1));

            if (fileStoringManager.DeleteFile(fileId))
            {
                response.StatusCode = 200;
                response.StatusDescription = "OK";
            }
            else
            {
                response.StatusCode = 404;
                response.StatusDescription = "File with such id not found!";
            }

            response.OutputStream.Close();
        }

        public void Close()
        {
            isListen = false;
            if (httpListener != null)
            {
                httpListener.Stop();
                httpListener = null;
            }
            threadListen.Abort();
        }
    }

}
