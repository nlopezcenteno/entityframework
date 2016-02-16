using System;
using System.Collections.Generic;
using System.Linq;
using UnitTesting.Core.Models;

namespace UnitTesting.Core
{
    public class UniversityManager
    {
        private IUniversityContextFactory _contextFactory;

        public UniversityManager(IUniversityContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public UniversityManager(string nameOrConnectionString)
        {
            _contextFactory = new UniversityContextFactory(nameOrConnectionString);
        }

        public Student GetStudentById(int id)
        {
            using (var db = _contextFactory.CreateContext())
            {
                return db.Students.FirstOrDefault(s => s.Id == id);
            }
        }

        public IEnumerable<Student> GetStudentsByCourse(int courseId)
        {
            using (var db = _contextFactory.CreateContext())
            {
                return db.Enrollments
                    .Where(e => e.CourseId == courseId)
                    .Select(e => e.Student)
                    .OrderBy(s => s.LastName).ToList();
            }
        }

        public Student CreateStudent(string firstName, string lastName, DateTime enrollmentDate)
        {
            var student = new Student
            {
                FirstName = firstName.Crop(50),
                LastName = lastName.Crop(50),
                EnrollmentDate = enrollmentDate
            };

            using (var db = _contextFactory.CreateContext())
            {
                db.Students.Add(student);
                db.Save();
                return student;
            }
        }

        public Course GetCourseById(int id)
        {
            using (var db = _contextFactory.CreateContext())
            {
                return db.Courses.FirstOrDefault(s => s.Id == id);
            }
        }

        public IEnumerable<Course> GetAllCourses()
        {
            using (var db = _contextFactory.CreateContext())
            {
                return db.Courses.OrderBy(c => c.Name).ToList();
            }
        }

        public Course CreateCourse(string name, int credits)
        {
            var course = new Course
            {
                Name = name.Crop(50),
                Credits = credits,
            };

            using (var db = _contextFactory.CreateContext())
            {
                db.Courses.Add(course);
                db.Save();
                return course;
            }
        }

        public Enrollment EnrollStudentToCourse(int studentId, int courseId)
        {
            using (var db = _contextFactory.CreateContext())
            {
                var student = db.Students.FirstOrDefault(s => s.Id == studentId);

                if (student == null)
                    throw new Exception("Student not found");

                var course = db.Courses.FirstOrDefault(c => c.Id == courseId);

                if (course == null)
                    throw new Exception("Course not found");

                var enrollment = new Enrollment
                {
                    StudentId = studentId,
                    CourseId = courseId
                };

                db.Enrollments.Add(enrollment);
                db.Save();
                return enrollment;
            }
        }
    }
}
