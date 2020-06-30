using SATNET.Domain;
using SATNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SATNET.Repository.Core.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }
        public void BeginTransaction();
        public void SaveChanges();
        public void Rollback();

        IRepository<Customer> Customers { get; }
        IRepository<ServicePlan> ServicePlans { get; }
        IRepository<ServicePlanPrice> ServicePlanPrices { get; }
        IRepository<Site> Sites { get; }
    }
}
