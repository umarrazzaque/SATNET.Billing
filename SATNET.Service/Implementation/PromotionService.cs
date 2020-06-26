using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class PromotionService : IService<Promotion>
    {
        private readonly IRepository<Promotion> _promotionRepository;
        public PromotionService(IRepository<Promotion> promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }
        public async Task<Promotion> Get(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Promotion>> List(Promotion obj)
        {
            return await _promotionRepository.List(obj);
        }
        public async Task<StatusModel> Add(Promotion Promotion)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Update(Promotion Promotion)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

    }
}
