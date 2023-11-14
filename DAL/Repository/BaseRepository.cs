using BL.IRepository;
using CoreliaTask.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class BaseRepository<T>:IBaseRepository<T> where T : class
    {
        protected AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public T Add(T entity)
        {
             _context.Set<T>().Add(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
           
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().SingleOrDefault(predicate);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, int take, int skip)
        {
            return _context.Set<T>().Where(predicate).Skip(skip).Take(take).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList(); 
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }
    }
}
