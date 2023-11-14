using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BL.IRepository
{
    public interface IBaseRepository<T>where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        T Find(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, int take, int skip);
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
