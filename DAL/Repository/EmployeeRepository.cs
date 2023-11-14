using BL.IRepository;
using CoreliaTask.Data;
using CoreliaTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>,IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }
    }
}
