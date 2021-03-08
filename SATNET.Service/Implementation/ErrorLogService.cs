using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace SATNET.Service.Implementation
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly IRepository<ErrorLog> _errorLogRepository;
        public ErrorLogService(IRepository<ErrorLog> errorLogRepository)
        {
            _errorLogRepository = errorLogRepository;
        }
        public async Task Add(ErrorLog obj)
        {
            try
            {
                await _errorLogRepository.Add(obj);
            }
            catch (Exception e)
            {
            }
        }
    }
}
