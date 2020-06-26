using System.Collections.Generic;

namespace server2._0
{
    public interface Servlet
    {
        public string GET(string resource, Dictionary<string, string> param_value);
       
    }
}