﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demጽ.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetFirst(int count);
        Task<List<T>> GetLast(int count);
        Task<T> Get(string id);
        Task<T> Add(T entity);
        Task<T> Update(T entit);
        Task<T> Delete(String id);

    }
}
