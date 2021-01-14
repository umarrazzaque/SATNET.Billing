using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Service.Configuration
{
    public class APISettings
    {
        public static string BuildAPIUrl(string type, string siteName)
        {
            string url = "";
            if (type == "lock")
            {
                url = "http://10.2.0.50/rest/modem/usatcom/" + siteName + "/lock";
                //url = "http://10.2.0.50/rest/modem/usatcom/API001/lock";
            }
            else if (type == "unlock")
            {
                url = "http://10.2.0.50/rest/modem/usatcom/" + siteName + "/unlock";
            }
            else if (type == "token")
            {
                url = "http://10.2.0.27/qm/rest/subscriptions/subscribe";
            }
            return url;
        }

    }
}
