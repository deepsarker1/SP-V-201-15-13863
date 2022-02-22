using DeepWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DeepWebApp.DBContexts
{
    public class StudentDBContext:DbContext
    {
        public StudentDBContext(DbContextOptions<StudentDBContext> options) : base(options)
        {

        }

        public DbSet<Student> Student { get; set; }
    }
}
