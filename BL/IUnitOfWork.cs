using BL.IRepository;
using CoreliaTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IUnitOfWork:IDisposable
    {
        IBaseRepository<Employee> Employees { get; }
        IEmployeeRepository Employee { get; }
        void Save();
    }
}
