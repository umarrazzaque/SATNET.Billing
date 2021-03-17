using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace SATNET.Service.Implementation
{
    public class CodeErrorLogService : ICodeErrorLogService
    {
        private readonly ICodeErrorLogRepository _errorLogRepository;
        public CodeErrorLogService(ICodeErrorLogRepository errorLogRepository)
        {
            _errorLogRepository = errorLogRepository;
        }
        public async Task Add(CodeErrorLog obj)
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
