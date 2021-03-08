using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Interface
{
    public interface IErrorLogService
    {
        public Task Add(ErrorLog obj);
    }
}
