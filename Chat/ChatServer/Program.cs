using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Program
    {
        public static Server server = new Server();
        static void Main(string[] args)
        {
            server.StartListen();
        }
    }
}
