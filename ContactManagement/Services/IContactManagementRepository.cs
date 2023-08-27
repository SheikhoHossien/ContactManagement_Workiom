using ContactManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Services
{
    public interface IContactManagementRepository<TEntity>
    {
        IList<TEntity> List();
        TEntity Find(int id);
        List<TEntity> Add(List<TEntity> entity);
        TEntity Update(int id, TEntity entity);
        void Delete(int id);
        //string AddColumns(List<Column> entity);
        List<TEntity> Search(string term);
        
    }
}
