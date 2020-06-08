using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Repository.Helper
{
    class Parse
    {
        public static Int32 ToInt32(object val)
        {
            if (val == DBNull.Value || val == null)
                return 0;
            return String.IsNullOrEmpty(val.ToString()) ? 0 : Convert.ToInt32(val);
        }
    }
}
