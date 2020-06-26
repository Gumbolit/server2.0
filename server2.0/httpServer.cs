using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace VideoServer
{
    class httpServer
    {
        public const String MSG_DIR = "/root/msg/";
        public const String WEB_DIR = "/root/web";
        public const String version = "HTTP/1.0";
        public const String servername = "myserver/1.1";

        TcpListener listner;
        bool running = false;

        public httpServer(int port)
        {
            listner = new TcpListener(IPAddress.Any, port);
        }

        public void start()
        {
            listner.Start();
            running = true;
            Console.WriteLine("server running");
            while (running)
            {
                Console.WriteLine("server waiting conecnting");
                TcpClient client = listner.AcceptTcpClient();
                Console.WriteLine("client conecnting");
                HandlClient(client);
                client.Close();
            }
        }

        void HandlClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());
            String msg = "";
            while (reader.Peek() != -1)
            {
                String line =  reader.ReadLine() ;
                msg += line +"\n";//наш принятый http get запрос
            }

            Console.WriteLine("REQUEST:\r\n {0}", msg);
            Request request = Request.GetReqest(msg);
            Response response = Response.From(request);
            response.post(client.GetStream());
        }
    }
}
