using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SATNET.Service.Implementation
{
    public class BackgroundTaskService : IBackgroundTaskService
    {
        private readonly IService<Site> _siteService;
        private readonly IBackgroundTaskRepository _backgroundTaskRepository;
        public BackgroundTaskService(IBackgroundTaskRepository backgroundTaskRepository)
        {
            _backgroundTaskRepository = backgroundTaskRepository;
            _siteService = new SiteService();
        }
        public void InsertMRCInvoice()
        {
            try
            {
                var sites = _siteService.List(new Site() { StatusId = 17 }).Result;
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

    }
}
