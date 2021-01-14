using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Interface
{
    public interface IAPIService
    {
        public bool LockUnlockSite(string siteName, string requestType);
        public bool TokenTopUpSite(string siteName, string bundleName);
    }
}
