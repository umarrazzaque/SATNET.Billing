using SATNET.Repository.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Repository.Core.Base
{
    public class RepositoryBase
    {
        public IUnitOfWork UnitOfWork = null;
        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }
    }
}
