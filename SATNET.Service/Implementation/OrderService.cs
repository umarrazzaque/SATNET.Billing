using Microsoft.VisualBasic;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class OrderService : IService<Order>
    {
        private readonly IRepository<Order> _orderRepository;
        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<Order> Get(int id)
        {
            var retModel = new Order();
            try
            {
                retModel = await _orderRepository.Get(id);
                if (retModel.Id != 0)
                {
                    retModel.ServiceProRataPrice = CalculateProRataPrice(retModel);
                    retModel.Total = retModel.ServiceProRataPrice + retModel.HardwarePrice + retModel.IPPrice;
                    retModel.ServicePlanUnit = SetServicePlanUnit(retModel.ServicePlanTypeId);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {

            }
            return retModel;
        }
        public async Task<List<Order>> List(Order obj)
        {
            return await _orderRepository.List(obj);
        }
        public async Task<StatusModel> Add(Order order)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Order/Index" };
            try
            {
                int retId = await _orderRepository.Add(order);
                if (retId != 0)
                {
                    status.IsSuccess = true;
                    status.ErrorCode = "Record inserted successfully.";
                }
                else
                {
                    status.IsSuccess = false;
                    status.ErrorCode = "Error in inserting the record.";
                }
            }
            catch (Exception e)
            {
                status.IsSuccess = false;
                status.ErrorCode = "An error occured while processing the request.";
                status.ErrorDescription = e.Message;
            }
            finally
            {

            }
            return status;
        }
        public async Task<StatusModel> Update(Order order)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

        private decimal CalculateProRataPrice(Order obj)
        {
            int installationMonth = DateAndTime.Month(obj.InstallationDate);
            int installationDay = DateAndTime.Day(obj.InstallationDate);
            int daysPerMonth = DateTime.DaysInMonth(2020, installationMonth);
            int remainingDays = daysPerMonth - installationDay + 1;
            decimal proRataPrice = (obj.ServicePlanPrice / 30) * remainingDays;
            return decimal.Round(proRataPrice, 2, MidpointRounding.AwayFromZero);
        }

        private string SetServicePlanUnit(int servicePlanTypeId)
        {
            string servicePlanUnit = "";
            if (servicePlanTypeId == 12)
            {//quota
                servicePlanUnit = ServiceUnit.GBPerMonth;
            }
            else if (servicePlanTypeId == 14)
            {//dedicated
                servicePlanUnit = ServiceUnit.Mbps;
            }
            return servicePlanUnit;
        }
    }
}
