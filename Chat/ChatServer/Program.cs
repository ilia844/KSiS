using System;
using FileStoringService;

namespace ChatServer
{
    class Program
    {
        private const string FileSharingServerUrl = "http://localhost:8888/";


        public static Server server = new Server();
        public static FileSharingServer fileSharingServer = new FileSharingServer(FileSharingServerUrl);
        static void Main(string[] args)
        {
//            server.LoadDB();
            server.StartListen();
            fileSharingServer.Start();
        }
    }
}
