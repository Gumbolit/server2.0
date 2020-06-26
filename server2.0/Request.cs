using System;
using System.Collections.Generic;
using System.Text;

namespace VideoServer
{
    class Request
    {
        public String type { get; set; }
        public String resource { get; set; }
        public String host { get; set; }
        public Dictionary<string, string> param_value { get; set; }

        private Request(String type, String host, String resource, Dictionary<string, string> param_value)
        {
            this.type = type;
            this.resource = resource;
            this.host = host;
            this.param_value = param_value;

        }

        public static Request GetReqest(String msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return null;
            }
            string[] tokens = msg.Split(" ");
            string[] URLtokens1 = tokens[1].Split('?');
            string resource = URLtokens1[0];
            Dictionary<string, string> param_value = new Dictionary<string, string>();
            if (URLtokens1.Length > 1)
            {
                string[] URLtokens2 = URLtokens1[1].Split('&');

                for (int j = 0; j < URLtokens2.Length; j++)
                {
                    string[] URLtokens3 = URLtokens2[j].Split('=');
                    param_value.Add(URLtokens3[0], URLtokens3[1]);
                }
                Console.WriteLine("type-{0}; resource-{1}; host-{2}; param_value-{3}", tokens[0], URLtokens1[0], tokens[3], param_value);
                return new Request(tokens[0], tokens[3], URLtokens1[0], param_value);
            }
            else
            {
                Console.WriteLine("type-{0}; url-{1}; host-{2}", tokens[0], tokens[1], tokens[3]);
                return new Request(tokens[0], tokens[3], tokens[1], param_value);
            }
        }
    }
}
