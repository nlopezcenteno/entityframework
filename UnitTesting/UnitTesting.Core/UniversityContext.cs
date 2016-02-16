using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using UnitTesting.Core.Models;

namespace UnitTesting.Core
{
    internal class UniversityContext : DbContext, IUniversityContext
    {
        public UniversityContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public void Save()
        {
            try
            {
                SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Trace.TraceError(ex.DbEntityValidationExceptionToString());
                throw;
            }
        }
    }
}
