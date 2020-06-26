using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Reflection;
using server2._0;

namespace VideoServer
{
    class Response
    {
        Byte[] data;
        string status;
        string mime;
        private Response(string status, String mime, byte[] data)
        {
            this.data = data;
            this.status = status;
            this.mime = mime;

        }

        static Servlet find_servlet(string resurseName, Dictionary<string, string> param_value)
        {
            var asmbly = Assembly.GetExecutingAssembly();
            Type type = asmbly.GetTypes().Where(
                    t => t.GetCustomAttributes(typeof(RequestHandlerAttribute), true).Length > 0 //из всех типов сборки находим содержащие атрибут RequestHandler 
            ).Where(
                v => v.GetCustomAttribute<RequestHandlerAttribute>().path == resurseName// из найденый выбираем с нужным параметром path
                ).FirstOrDefault();
            if (type == null) return null;
            //ConstructorInfo info = type.GetConstructor(new Type[] { typeof(string), typeof(Dictionary<string, string>) });
           // object servlet = info.Invoke(new object[] { resurseName, param_value });
            var servlet = Activator.CreateInstance(type);
            return (Servlet)servlet;
        }

        public static Response From(Request request)
        {
           
            if (request == null)
            {
                return NotWork("400.html", "400 Bad Request");
            }

            if (request.type == "GET")
            {
                Servlet servlet = find_servlet(request.resource,request.param_value);

                if (servlet != null)
                {
                    return new Response("200 OK", "text/html", System.Text.Encoding.Unicode.GetBytes(servlet.GET(request.resource, request.param_value)));
                }
                String file = Environment.CurrentDirectory + httpServer.WEB_DIR + request.resource;
                FileInfo fi = new FileInfo(file);
                if (fi.Exists && fi.Extension.Contains("."))
                {
                    return MakeFromFile(fi);
                }
                else
                {
               
                    DirectoryInfo di = new DirectoryInfo(fi + "/");
                    if (!di.Exists)
                    {

                        return NotWork("404.html", "404 Page Not Found");
                    }
                    FileInfo[] files = di.GetFiles();
                    foreach (FileInfo ff in files)
                    {
                        if (ff.Name.Contains("default.html") || ff.Name.Contains("index.html"))
                            return MakeFromFile(ff);
                    }
                }
            }
            else
            {
                return NotWork("405.html", "405 Method Not Allowed");
            }
            return NotWork("404.html", "404 Page Not Found");
        }
      
        private static Response MakeFromFile(FileInfo fi)
        {
            FileStream fs = fi.OpenRead();
            Byte[] d = new Byte[fs.Length];
            BinaryReader reader = new BinaryReader(fs);
            reader.Read(d, 0, d.Length);

            return new Response("200 OK", "text/html", d);
        }

        private static Response NotWork(String fileName, String status)
        {

            String file = Environment.CurrentDirectory + httpServer.MSG_DIR + fileName;
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);


            return new Response(status, "text/html", d);
        }

        public void post(NetworkStream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            Console.WriteLine(String.Format("RESPONSE:\r\n{0} {1}\r\nServer: {2}\r\nContent-Language: ru\r\nContent-type:{3}\r\nAccept-Ranges: bytes\r\nContentLength: {4}\r\nConnection: close\r\n",
                httpServer.version, status, httpServer.servername, mime, data.Length));
            writer.WriteLine(String.Format("{0} {1}\r\nServer: {2}\r\nContent-Language: ru\r\nContent-type:{3}\r\nAccept-Ranges: bytes\r\nContentLength: {4}\r\nConnection: close\r\n",
                httpServer.version, status, httpServer.servername, mime, data.Length));
            writer.Flush();

            stream.Write(data, 0, data.Length);
        }


    }

}
