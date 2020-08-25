﻿using Microsoft.Extensions.Configuration;
using SATNET.Domain;
using SATNET.Repository.Core.Interface;
using SATNET.Repository.Implementation;
using SATNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SATNET.Repository.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDbConnection _connection = null;
        public IDbTransaction _transaction = null;

        public UnitOfWork()
        {
            IDbConnection dbConnection = new SqlConnection("Server=.;Database=SatnetBilling;Trusted_Connection=True;MultipleActiveResultSets=true");//local
            //IDbConnection dbConnection = new SqlConnection("Server=tcp:satnetbilling.database.windows.net,1433;Initial Catalog=SatnetBilling;Persist Security Info=False;User ID=usat;Password=Password01@@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");//azure
            _connection = dbConnection;
            OpenConnection();
        }

        public IDbConnection Connection
        {
            get { return _connection; }
        }
        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed) { _connection.Open(); }
            else { _connection.Open(); }   
        }

        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void SaveChanges()
        {
            if (_transaction == null)
                return; //   throw new TransactionAlreadyClosedException();

            _transaction.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        private IRepository<Customer> _customerRepository;
        public IRepository<Customer> Customers { get { return _customerRepository ?? (_customerRepository = new CustomerRepository(this)); } }

        private IRepository<ServicePlan> _serviceRepository;
        public IRepository<ServicePlan> ServicePlans { get { return _serviceRepository ?? (_serviceRepository = new ServicePlanRepository(this));  } }
        
        private IRepository<ServicePlanPrice> _servicePlanPriceRepository;
        public IRepository<ServicePlanPrice> ServicePlanPrices { get { return _servicePlanPriceRepository ?? (_servicePlanPriceRepository = new ServicePlanPriceRepository(this)); } }

        private IRepository<Site> _siteRepository;
        public IRepository<Site> Sites { get { return _siteRepository ?? (_siteRepository = new SiteRepository(this)); } }

        private IRepository<Lookup> _lookuoRepository;
        public IRepository<Lookup> Lookups { get { return _lookuoRepository ?? (_lookuoRepository = new LookupRepository(this)); } }
    }
}
