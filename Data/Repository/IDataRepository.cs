using System;
using System.Collections.Generic;

namespace ValeoBot.Data.Repository
{
    public interface IDataRepository<TEntity>
    {
        IEnumerable<TEntity> All { get; }
        TEntity Get(long id);
        TEntity Add(TEntity entity);
        TEntity[] Find(Func<TEntity, bool> predicator);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}