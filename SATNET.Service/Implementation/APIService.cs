using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using SATNET.Service.Configuration;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SATNET.Service.Implementation
{
    public class APIService : IAPIService
    {
        private readonly IConfiguration _config;
        private readonly string lockAPIUserName;
        private readonly string lockAPIPassword; 
        private readonly string tokenAPIUserName;
        private readonly string tokenAPIPassword;
        public APIService()
        {
        }
        public APIService(IConfiguration config)
        {
            _config = config;
            lockAPIUserName = _config.GetSection("APISettings").GetSection("LockAPISettings").GetSection("UserName").Value;
            lockAPIPassword = _config.GetSection("APISettings").GetSection("LockAPISettings").GetSection("Password").Value; 
            tokenAPIUserName = _config.GetSection("APISettings").GetSection("TokenAPISettings").GetSection("UserName").Value;
            tokenAPIPassword = _config.GetSection("APISettings").GetSection("TokenAPISettings").GetSection("Password").Value;
        }
        public bool LockUnlockSite(string siteName, string requestType)
        {
            try
            {
                RestClient lockClient = new RestClient(APISettings.BuildAPIUrl("lock", siteName));
                lockClient.Authenticator = new HttpBasicAuthenticator(lockAPIUserName, lockAPIPassword);
                var request = new RestRequest();
                request.Method = requestType == "lock" ? Method.POST : requestType == "unlock" ? Method.DELETE : Method.POST;
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                //request.AddParameter("application/json", "{}", ParameterType.RequestBody);
                IRestResponse response = lockClient.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        public bool TokenTopUpSite(string siteName, string token)
        {
            try
            {
                RestClient lockClient = new RestClient(APISettings.BuildAPIUrl("token", siteName));
                lockClient.Authenticator = new HttpBasicAuthenticator(tokenAPIUserName, tokenAPIPassword);
                var request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(new { terminalName = siteName, terminalDomainName = "USatcom", bundleName = "2GB-TokenHigh", activateNow = true, expirationTime = DateTime.Now.AddMonths(4) }); // uses JsonSerializer
                IRestResponse response = lockClient.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
