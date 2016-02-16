using System;
using System.Data.Entity;
using UnitTesting.Core.Models;

namespace UnitTesting.Core
{
    public interface IUniversityContext : IDisposable
    {
        DbSet<Course> Courses { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Enrollment> Enrollments { get; set; }

        void Save();
    }
}
