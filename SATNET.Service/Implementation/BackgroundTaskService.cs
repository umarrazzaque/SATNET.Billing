using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SATNET.Repository.Implementation;
using Microsoft.Extensions.Configuration;
using SATNET.Repository.Core;

namespace SATNET.Service.Implementation
{
    public class BackgroundTaskService : IBackgroundTaskService
    {
        private readonly IService<Site> _siteService;
        private readonly IService<Order> _orderService;
        private readonly IBackgroundTaskRepository _backgroundTaskRepository;
        private readonly IAPIService _APIService;
        public BackgroundTaskService(IBackgroundTaskRepository backgroundTaskRepository, IConfiguration configuration)
        {
            _backgroundTaskRepository = backgroundTaskRepository;
            _siteService = new SiteService();
            _orderService = new OrderService(new OrderRepository(configuration), new SiteRepository(new UnitOfWork()) , new APIService(configuration), new LookupRepository(new UnitOfWork()));
            _APIService = new APIService(configuration);
        }
        public void InsertMRCInvoice()
        {
            try
            {
                var sites = _siteService.List(new Site() { StatusId = 17, NextBillingDate = DateTime.Now.Date }).Result;
                if (sites != null && sites.Count > 0)
                {
                    //sites = sites.Where(s => s.CustomerId == 12).ToList();
                    foreach (var site in sites)
                    {
                        _backgroundTaskRepository.InsertMRCInvoice(site.Id);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void InsertMRCInvoiceMonthly()
        {
            try
            {
                var sites = _siteService.List(new Site() { StatusId = 17, NextBillingDate=DateTime.Now.Date }).Result;
                if (sites != null && sites.Count > 0)
                {
                    //sites = sites.Where(s => s.CustomerId == 12).ToList();
                    foreach (var site in sites)
                    {
                        _backgroundTaskRepository.InsertMRCInvoice(site.Id);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void LockSitesEndOfMonth()
        {
            try
            {
                var orders = _orderService.List(new Order() { RequestTypeId = 6, Flag = "EndOfMonth", CreatedOn=DateTime.Now }).Result;
                if (orders != null && orders.Count > 0)
                {
                    foreach (var o in orders)
                    {
                        try
                        {
                            bool isSuccess = _APIService.LockUnlockSite(o.SiteName, "lock");
                            if (isSuccess)
                            {
                                o.StatusId = 21;
                                _orderService.Update(o); // complete order and update site status to locked
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void TerminateSitesEndOfMonth()
        {
            try
            {
                var sites = _siteService.List(new Site() { Flag = "Scheduled_Site_Termination" }).Result;
                if (sites != null && sites.Count > 0)
                {
                    foreach (var s in sites)
                    {
                        try
                        {
                            bool isSuccess = _siteService.Update(new Site() {Id=s.Id, Flag = "UpdateStatus", StatusId = 19 }).Result.IsSuccess; // update site status to terminated
                            if (isSuccess)
                            {
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
