using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SATNET.Service.Implementation
{
    public class ExceptionService
    {
        public ExceptionService()
        {

        }

        public StatusModel HandleException(Exception exception)
        {
            StatusModel retModel = new StatusModel();
            SqlException sqlException = (SqlException)exception.InnerException;
            if (sqlException.Number == 2627)
            {
                retModel.ErrorCode = "An error occured due to unique key constraint.";
            }
            else
            {
                retModel.ErrorCode = "An error occured while processing request.";
            }
            return retModel;
        }
    }
}
