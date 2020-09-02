using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Site : BaseEntity
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public string SubscriberCity { get; set; }
        public string SubscriberArea { get; set; }
        public string SubscriberEmail { get; set; }
        public string SubscriberNotes { get; set; }
        public int IPId { get; set; }
        public int ServicePlanId { get; set; }
        public int ServicePlanTypeId { get; set; }
        public string IPName { get; set; }
        public int StatusId { get; set; }
        public List<int> StatusIds { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public int AreaId { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string  Subscriber{ get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime LastActivationDate { get; set; }
        public int MacAirNoId { get; set; }
        public int PromotionId { get; set; }
        public int HardwareConditionId { get; set; }
    }
}
