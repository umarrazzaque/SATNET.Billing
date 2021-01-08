﻿using Microsoft.VisualBasic;
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
        private readonly IAPIService _APIService;
        public OrderService()
        {
        }
        public OrderService(IRepository<Order> orderRepository, IAPIService APIService)
        {
            _orderRepository = orderRepository;
            _APIService = APIService;
        }
        public async Task<Order> Get(int id)
        {
            var retModel = new Order();
            try
            {
                retModel = await _orderRepository.Get(id);
                //if (retModel != null && retModel.Id != 0)
                //{
                //    retModel.ServiceProRataPrice = CalculateProRataPrice(retModel);
                //    retModel.Total = retModel.ServiceProRataPrice + retModel.HardwarePrice + retModel.IPPrice;
                //    retModel.ServicePlanUnit = SetServicePlanUnit(retModel.ServicePlanTypeId);
                //}
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
                    // check condition for lock/unlock/token 
                    if ((order.RequestTypeId == 6 || order.RequestTypeId == 7) && order.ScheduleDateId == 58) //request type: lock/unlock, schedulate date:now
                    {
                        var orderDetails = await _orderRepository.Get(retId);
                        status = await LockUnLockSite(orderDetails);
                        return status;
                    }
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Order/Index" };
            try
            {
                int retId = 0;
                retId = await _orderRepository.Update(order);
                if (retId != 0)
                {
                    status.IsSuccess = true;
                    status.ErrorCode = "Order updated successfully.";
                }
                else
                {
                    status.IsSuccess = false;
                    status.ErrorCode = "Error in updating the record.";
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
        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Order/Index" };
            try
            {
                int dRow = await _orderRepository.Delete(id, deletedBy);
                if (dRow > 0)
                {
                    status.IsSuccess = true;
                    status.ErrorCode = "Service order has been cancelled successfully.";
                }
                else
                {
                    status.ErrorCode = "An error occured while processing request.";
                }
            }
            catch (Exception e)
            {
                status.ErrorCode = "An error occured while processing request.";
            }
            finally
            {
            }
            return status;
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
        private async Task<StatusModel> LockUnLockSite(Order order)
        {
            int retId = 0;
            bool apiResult = false;
            string requestType = "";
            var status = new StatusModel();
            requestType = order.RequestTypeId == 6 ? "lock" : order.RequestTypeId==7? "unlock":"";
            apiResult = _APIService.LockUnlockSite(order.SiteName, requestType);
            if (apiResult)
            {
                order.StatusId = 21; // complete order
                retId = await _orderRepository.Update(order);
                if (retId != 0)
                {
                    status.IsSuccess = true;
                    status.ErrorCode = "Site locked successfully.";
                }
                else
                {
                    status.IsSuccess = false;
                    status.ErrorCode = "Error in updating lock status of site.";
                }
            }
            else
            {
                status.IsSuccess = false;
                status.ErrorCode = "Some error occurred while calling to lock/unlock API.";
            }
            return status;
        }
    }
}
