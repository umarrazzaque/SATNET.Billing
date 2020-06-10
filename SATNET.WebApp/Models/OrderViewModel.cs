using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class OrderViewModel
    {
        public int SiteId { get; set; }
        public DateTime ServiceOrderDate { get; set; }
        public int RequestTypeId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCity { get; set; }
        public string Notes { get; set; }
        public DateTime? UpgradeFrom { get; set; }
        public DateTime? UpgradeTo { get; set; }
        public DateTime? DowngradeFrom { get; set; }
        public DateTime? DowngradeTo { get; set; }
        public string Token { get; set; }
        public string Promotion { get; set; }
        public string Other { get; set; }
        public SelectList SiteList { get; set; }
    }
    //public class CreateOrderViewModel
    //{
    //    public CreateOrderViewModel()
    //    {
    //        OrderViewModel = new OrderViewModel();
    //        DistributorList = new List<DistributorViewModel>();
    //    }
    //    public OrderViewModel OrderViewModel { get; set; }
    //    public List<DistributorViewModel> DistributorList { get; set; }
    //}
    //public class OrderListViewModel
    //{
    //    public OrderListViewModel()
    //    {
    //        OrderList = new List<OrderViewModel>();
    //    }
    //    public List<OrderViewModel> OrderList { get; set; }
    //}
}
