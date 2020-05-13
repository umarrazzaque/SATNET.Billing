using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Repository
{
    public class Common
    {
        private  readonly IConfiguration _config;
        public Common(IConfiguration config)
        {
            _config = config;
        }
        //public static string GetConnectionString()
        //{
        //    string conString;
        //    conString= ConfigurationExtensions.GetConnectionString(_config, "DefaultConnection");
        //    return conString;
        //}
    }
}
