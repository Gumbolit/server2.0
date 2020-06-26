using System;
using System.Collections.Generic;
using System.Text;

namespace server2._0
{
    [RequestHandler(path = "/hello")]
    public class user_class : Servlet
    {

        public user_class()
       {

       }

        public string GET(string resource, Dictionary<string, string> param_value)
        {
            return "<html><body><h1> "+ param_value["Name1"] + " </h1></body></html>";
        }
    }
}
