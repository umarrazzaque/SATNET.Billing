using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SATNET.Repository.Core
{
    public class DatabaseContext
    {
        private readonly bool _ownsConnection;
        //private readonly OracleConnection _connection2=  new OracleConnection() ;
        private SqlConnection _connection;
        private IDbTransaction _transaction;
    }
}
