using Microsoft.Extensions.Configuration;
using SATNET.Repository.Core.Base;
using SATNET.Repository.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SATNET.Repository.Core
{
    public class UnitOfWorkFactory
    {
        private readonly IConfiguration _config;
        private IDbConnection _connection = null;
        private UnitOfWork _unitOfWork = null;
        public IUnitOfWork Create()
        {
            //_config = config;
            //_connection = new SqlConnection("Server=.\\SQLExpress;Database=SatnetBilling;Trusted_Connection=True;MultipleActiveResultSets=true");
            
            UnitOfWork = new UnitOfWork();
            return UnitOfWork;
        }

        private string GetConnectionString(IConfiguration configuration)
        {
            return "";
        }
        public UnitOfWorkFactory()
        {
            
        }

        public UnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
            set { _unitOfWork = value; }
        }
        
    }
}
