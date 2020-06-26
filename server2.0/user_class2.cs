using System;
using System.Collections.Generic;
using System.Text;

namespace server2._0
{
    [RequestHandler(path = "/hell")]
    public class user_class2 : Servlet
    {
       public user_class2()
        {
         
        }

        public string GET(string resource, Dictionary<string, string> param_value)
        {
            StringBuilder sb = new StringBuilder("<html> <head> <TITLE>")
                .Append(resource)
                .Append("</TITLE> </head> <body> <h1> ")
                .Append(resource)
                .Append(" </h1> <table> <th> <td> ИМЯ </td> <td> ЗНАЧЕНИЕ </td> </th>");
            foreach (KeyValuePair<string, string> kvp in param_value)
                sb.Append(string.Format("<tr> <td> {0} </td> <td>{1}</td> </tr>",
                    kvp.Key, kvp.Value));

            sb.Append("</table> </body> </html> ");
            return sb.ToString();
        }
    }
}
