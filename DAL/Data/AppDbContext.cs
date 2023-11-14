
using CoreliaTask.Model;
using Microsoft.EntityFrameworkCore;

namespace CoreliaTask.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
