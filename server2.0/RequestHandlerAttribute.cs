using System;

namespace server2._0
{
    internal class RequestHandlerAttribute : Attribute
    {
       
            public string path { get; set; }

            public RequestHandlerAttribute()
            {
            }
            public RequestHandlerAttribute(string path)
            {
                this.path = path;
            }
       
    }
}