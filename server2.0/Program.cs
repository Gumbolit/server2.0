using System;


namespace VideoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            httpServer server = new httpServer(8080);
            server.start();
        }
    }
}