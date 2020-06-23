﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Interface
{
    public interface IService<T>
    {
        public Task<T> Get(int id);
        public Task<List<T>> List(T obj);
        public Task<StatusModel> Add(T obj);
        public Task<StatusModel> Update(T obj);
        public Task<StatusModel> Delete(int recId, int deletedBy);
    }
}
