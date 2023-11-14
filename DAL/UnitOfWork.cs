using BL;
using BL.IRepository;
using CoreliaTask.Data;
using CoreliaTask.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IBaseRepository<Employee> Employees { get; private set; }
        public IEmployeeRepository Employee { get; private set; }

        
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Employees = new BaseRepository<Employee>(_context);
            Employee = new EmployeeRepository(_context);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
