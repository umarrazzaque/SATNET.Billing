using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class TokenService : IService<Token>
    {
        private readonly IRepository<Token> _tokenRepository;
        public TokenService(IRepository<Token> tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }
        public async Task<Token> Get(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Token>> List(Token obj)
        {
            return await _tokenRepository.List(obj);
        }
        public async Task<StatusModel> Add(Token Token)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Update(Token Token)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

    }
}
