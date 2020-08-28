using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class CityService : IService<City>
    {
        private readonly IRepository<City> _cityRepository;
        public CityService(IRepository<City> cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public async Task<City> Get(int id)
        {
            var retModel = new City();
            try
            {
                retModel = await _cityRepository.Get(id);
            }
            catch (Exception e)
            {

            }
            finally
            {

            }
            return retModel;
        }
        public async Task<List<City>> List(City obj)
        {
            return await _cityRepository.List(obj);
        }
        public async Task<StatusModel> Add(City order)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Update(City order)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }
    }
}
