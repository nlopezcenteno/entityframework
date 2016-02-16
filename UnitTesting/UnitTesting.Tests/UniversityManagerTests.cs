using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTesting.Core;
using UnitTesting.Core.Models;
using Xunit;

namespace UnitTesting.Tests
{
    public class UniversityManagerTests
    {
        private IUniversityContext _context;
        private UniversityManager _manager;

        public UniversityManagerTests()
        {
            _context = Substitute.For<IUniversityContext>();
            var contextFactory = Substitute.For<IUniversityContextFactory>();
            contextFactory.CreateContext().Returns(_context);
            _manager = new UniversityManager(contextFactory);
        }

        [Fact]
        public void GetStudentByIdFound()
        {
            // Arrange
            var studentId = 1;
            var firstName = "Test1";
            var lastName = "Test2";
            var enrollmentDate = DateTime.Today;
            var data = new List<Student>
            {
                new Student
                {
                    Id = studentId,
                    FirstName = firstName,
                    LastName = lastName,
                    EnrollmentDate = enrollmentDate
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Test3",
                    LastName = "Test4",
                    EnrollmentDate = DateTime.Today
                }
            };
            var dbSet = EntityFrameworkTestHelpers.MockDbSet(data);
            _context.Students.Returns(dbSet);

            // Act
            var student = _manager.GetStudentById(studentId);

            // Assert
            Assert.NotNull(student);
            Assert.Equal(studentId, student.Id);
            Assert.Equal(firstName, student.FirstName);
            Assert.Equal(lastName, student.LastName);
            Assert.Equal(enrollmentDate, student.EnrollmentDate);
        }

        [Fact]
        public void GetStudentByIdNotFoundReturnsNull()
        {
            // Arrange
            var studentId = 1;
            var data = new List<Student>
            {
                new Student
                {
                    Id = 2,
                    FirstName = "Test1",
                    LastName = "Test2",
                    EnrollmentDate = DateTime.Today
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Test3",
                    LastName = "Test4",
                    EnrollmentDate = DateTime.Today
                }
            };
            var dbSet = EntityFrameworkTestHelpers.MockDbSet(data);
            _context.Students.Returns(dbSet);

            // Act
            var student = _manager.GetStudentById(studentId);

            // Assert
            Assert.Null(student);
        }

        [Fact]
        public void GetStudentsByCourseFound()
        {
            // Arrange
            var courseId = 1;
            var dataStudents = new List<Student>
            {
                new Student
                {
                    Id = 1,
                    FirstName = "Name1",
                    LastName = "Last1",
                    EnrollmentDate = DateTime.Today
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Name2",
                    LastName = "Last2",
                    EnrollmentDate = DateTime.Today
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Name3",
                    LastName = "Last3",
                    EnrollmentDate = DateTime.Today
                }
            };
            var dataEnrollments = new List<Enrollment> {
                new Enrollment { CourseId = courseId, StudentId = 1, Student = dataStudents.Find(s=>s.Id == 1) },
                new Enrollment { CourseId = 2, StudentId = 1, Student = dataStudents.Find(s=>s.Id == 1) },
                new Enrollment { CourseId = 2, StudentId = 2, Student = dataStudents.Find(s=>s.Id == 2) },
                new Enrollment { CourseId = courseId, StudentId = 3, Student = dataStudents.Find(s=>s.Id == 3)}
            };
            var dbSetEnrollments = EntityFrameworkTestHelpers.MockDbSet(dataEnrollments);
            _context.Enrollments.Returns(dbSetEnrollments);

            // Act
            var students = _manager.GetStudentsByCourse(courseId);

            // Assert
            Assert.NotNull(students);
            Assert.NotEmpty(students);
            Assert.Equal(2, students.Count());
        }


        [Fact]
        public void GetStudentsByCourseIsOrderedByLastNameAsc()
        {
            // Arrange
            var courseId = 1;
            var lastName1 = "Last1";
            var lastName2 = "Last2";
            var lastName3 = "Last3";
            var dataStudents = new List<Student>
            {
                new Student
                {
                    Id = 1,
                    FirstName = "Name1",
                    LastName = lastName3,
                    EnrollmentDate = DateTime.Today
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Name2",
                    LastName = lastName2,
                    EnrollmentDate = DateTime.Today
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Name3",
                    LastName = lastName1,
                    EnrollmentDate = DateTime.Today
                }
            };
            var dataEnrollments = new List<Enrollment> {
                new Enrollment { CourseId = courseId, StudentId = 1, Student = dataStudents.Find(s=>s.Id == 1) },
                new Enrollment { CourseId = courseId, StudentId = 2, Student = dataStudents.Find(s=>s.Id == 2) },
                new Enrollment { CourseId = courseId, StudentId = 3, Student = dataStudents.Find(s=>s.Id == 3)}
            };
            var dbSetEnrollments = EntityFrameworkTestHelpers.MockDbSet(dataEnrollments);
            _context.Enrollments.Returns(dbSetEnrollments);

            // Act
            var students = _manager.GetStudentsByCourse(courseId);

            // Assert
            Assert.NotNull(students);
            Assert.NotEmpty(students);
            Assert.Equal(3, students.Count());
            Assert.Equal(lastName1, students.ElementAt(0).LastName);
            Assert.Equal(lastName2, students.ElementAt(1).LastName);
            Assert.Equal(lastName3, students.ElementAt(2).LastName);
        }


        [Fact]
        public void GetStudentsByCourseNotFoundReturnsEmptyCollection()
        {
            // Arrange
            var courseId = 1;
            var anotherCourseId = 2;
            var dataStudents = new List<Student>
            {
                new Student
                {
                    Id = 1,
                    FirstName = "Name1",
                    LastName = "Last1",
                    EnrollmentDate = DateTime.Today
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Name2",
                    LastName = "Last2",
                    EnrollmentDate = DateTime.Today
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Name3",
                    LastName = "Last3",
                    EnrollmentDate = DateTime.Today
                }
            };
            var dataEnrollments = new List<Enrollment> {
                new Enrollment { CourseId = anotherCourseId, StudentId = 1, Student = dataStudents.Find(s=>s.Id == 1) },
                new Enrollment { CourseId = anotherCourseId, StudentId = 2, Student = dataStudents.Find(s=>s.Id == 2) },
                new Enrollment { CourseId = anotherCourseId, StudentId = 3, Student = dataStudents.Find(s=>s.Id == 3)}
            };
            var dbSetEnrollments = EntityFrameworkTestHelpers.MockDbSet(dataEnrollments);
            _context.Enrollments.Returns(dbSetEnrollments);

            // Act
            var students = _manager.GetStudentsByCourse(courseId);

            // Assert
            Assert.NotNull(students);
            Assert.Empty(students);
        }


        [Fact]
        public void CreateStudent()
        {
            // Arrange
            var firstName = "Test1";
            var lastName = "Test2";
            var enrollmentDate = DateTime.Today;
            var dbSet = EntityFrameworkTestHelpers.MockDbSet<Student>();
            _context.Students.Returns(dbSet);

            // Act
            var student = _manager.CreateStudent(firstName, lastName, enrollmentDate);

            // Assert
            Assert.NotNull(student);
            Assert.Equal(firstName, student.FirstName);
            Assert.Equal(lastName, student.LastName);
            Assert.Equal(enrollmentDate, student.EnrollmentDate);
            _context.Students.Received(1).Add(Arg.Any<Student>());
            _context.Received(1).Save();
        }

        [Fact]
        public void CreateStudentCropFirstNameIfGreaterThan50()
        {
            // Arrange
            var firstName = new string('A', 51);
            var expectedFirstName = new string('A', 50);
            var dbSet = EntityFrameworkTestHelpers.MockDbSet<Student>();
            _context.Students.Returns(dbSet);

            // Act
            var student = _manager.CreateStudent(firstName, "Test2", DateTime.Today);

            // Assert
            Assert.NotNull(student);
            Assert.Equal(expectedFirstName, student.FirstName);
            _context.Students.Received(1).Add(Arg.Any<Student>());
            _context.Received(1).Save();
        }

        [Fact]
        public void CreateStudentCropLastNameIfGreaterThan50()
        {
            // Arrange
            var lastName = new string('A', 51);
            var expectedLastName = new string('A', 50);
            var dbSet = EntityFrameworkTestHelpers.MockDbSet<Student>();
            _context.Students.Returns(dbSet);

            // Act
            var student = _manager.CreateStudent("Test1", expectedLastName, DateTime.Today);

            // Assert
            Assert.NotNull(student);
            Assert.Equal(expectedLastName, student.LastName);
            _context.Students.Received(1).Add(Arg.Any<Student>());
            _context.Received(1).Save();
        }


        [Fact]
        public void GetCourseByIdFound()
        {
            // Arrange
            var courseId = 1;
            var name = "Course 1";
            var credits = 5;
            var data = new List<Course>
            {
                new Course
                {
                    Id = courseId,
                    Name = name,
                    Credits = credits
                },
                new Course
                {
                    Id = 2,
                    Name = "Course 2",
                    Credits = 7
                }
            };
            var dbSet = EntityFrameworkTestHelpers.MockDbSet(data);
            _context.Courses.Returns(dbSet);

            // Act
            var course = _manager.GetCourseById(courseId);

            // Assert
            Assert.NotNull(course);
            Assert.Equal(courseId, course.Id);
            Assert.Equal(name, course.Name);
            Assert.Equal(credits, course.Credits);
        }

        [Fact]
        public void GetCourseByIdNotFoundReturnsNull()
        {
            // Arrange
            var courseId = 1;
            var data = new List<Course>
            {
                new Course
                {
                    Id = 2,
                    Name = "Course 2",
                    Credits = 5
                },
                new Course
                {
                    Id = 3,
                    Name = "Course 3",
                    Credits = 7
                }
            };
            var dbSet = EntityFrameworkTestHelpers.MockDbSet(data);
            _context.Courses.Returns(dbSet);

            // Act
            var course = _manager.GetCourseById(courseId);

            // Assert
            Assert.Null(course);
        }

        [Fact]
        public void CreateCourse()
        {
            // Arrange
            var name = "Course 1";
            var credits = 5;
            var dbSet = EntityFrameworkTestHelpers.MockDbSet<Course>();
            _context.Courses.Returns(dbSet);

            // Act
            var course = _manager.CreateCourse(name, credits);

            // Assert
            Assert.NotNull(course);
            Assert.Equal(name, course.Name);
            Assert.Equal(credits, course.Credits);
            _context.Courses.Received(1).Add(Arg.Any<Course>());
            _context.Received(1).Save();
        }

        [Fact]
        public void CreateCourseCropNameIfGreaterThan50()
        {
            // Arrange
            var name = new string('A', 51);
            var expectedName = new string('A', 50);
            var dbSet = EntityFrameworkTestHelpers.MockDbSet<Course>();
            _context.Courses.Returns(dbSet);

            // Act
            var course = _manager.CreateCourse(name, 5);

            // Assert
            Assert.NotNull(course);
            Assert.Equal(expectedName, course.Name);
            _context.Courses.Received(1).Add(Arg.Any<Course>());
            _context.Received(1).Save();
        }


        [Fact]
        public void GetAllCoursesAreOrderedByNameAsc()
        {
            // Arrange
            var name1 = "Course A";
            var name2 = "Course B";
            var name3 = "Course C";
            var data = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = name3,
                    Credits = 5
                },
                new Course
                {
                    Id = 2,
                    Name = name1,
                    Credits = 7
                },
                new Course
                {
                    Id = 3,
                    Name = name2,
                    Credits = 5
                }
            };
            var dbSet = EntityFrameworkTestHelpers.MockDbSet(data);
            _context.Courses.Returns(dbSet);

            // Act
            var courses = _manager.GetAllCourses();

            // Assert
            Assert.NotNull(courses);
            Assert.Equal(3, courses.Count());
            Assert.Equal(name1, courses.ElementAt(0).Name);
            Assert.Equal(name2, courses.ElementAt(1).Name);
            Assert.Equal(name3, courses.ElementAt(2).Name);
        }

        [Fact]
        public void GetAllCoursesNotFoundReturnsEmptyCollection()
        {
            // Arrange
            var data = new List<Course>();
            var dbSet = EntityFrameworkTestHelpers.MockDbSet(data);
            _context.Courses.Returns(dbSet);

            // Act
            var courses = _manager.GetAllCourses();

            // Assert
            Assert.NotNull(courses);
            Assert.Empty(courses);
        }

        [Fact]
        public void EnrollStudentToCourse()
        {
            // Arrange
            var studentId = 1;
            var courseId = 1;
            var dataStudents = new List<Student>
            {
                new Student
                {
                    Id = studentId,
                    FirstName = "Test1",
                    LastName = "Test2",
                    EnrollmentDate = DateTime.Today
                }
            };
            var dataCourses = new List<Course>
            {
                new Course
                {
                    Id = courseId,
                    Name = "Course1",
                    Credits = 5
                }
            };
            var dbSetStudents = EntityFrameworkTestHelpers.MockDbSet(dataStudents);
            _context.Students.Returns(dbSetStudents);
            var dbSetCourses = EntityFrameworkTestHelpers.MockDbSet(dataCourses);
            _context.Courses.Returns(dbSetCourses);
            var dbSetEnrollments = EntityFrameworkTestHelpers.MockDbSet<Enrollment>();
            _context.Enrollments.Returns(dbSetEnrollments);

            // Act
            var enrollment = _manager.EnrollStudentToCourse(studentId, courseId);

            // Assert
            Assert.NotNull(enrollment);
            Assert.Equal(studentId, enrollment.StudentId);
            Assert.Equal(courseId, enrollment.CourseId);
            _context.Enrollments.Received(1).Add(Arg.Any<Enrollment>());
            _context.Received(1).Save();
        }

        [Fact]
        public void EnrollStudentToCourseThrowExceptionIfStudentNotFound()
        {
            // Arrange
            var expectedExceptionMessage = "Student not found";
            var studentId = 1;
            var anotherStudentId = 2;
            var courseId = 1;
            var dataStudents = new List<Student>
            {
                new Student
                {
                    Id = anotherStudentId,
                    FirstName = "Test1",
                    LastName = "Test2",
                    EnrollmentDate = DateTime.Today
                }
            };
            var dataCourses = new List<Course>
            {
                new Course
                {
                    Id = courseId,
                    Name = "Course1",
                    Credits = 5
                }
            };
            var dbSetStudents = EntityFrameworkTestHelpers.MockDbSet(dataStudents);
            _context.Students.Returns(dbSetStudents);
            var dbSetCourses = EntityFrameworkTestHelpers.MockDbSet(dataCourses);
            _context.Courses.Returns(dbSetCourses);
            var dbSetEnrollments = EntityFrameworkTestHelpers.MockDbSet<Enrollment>();
            _context.Enrollments.Returns(dbSetEnrollments);

            // Act
            var ex = Record.Exception(() => _manager.EnrollStudentToCourse(studentId, courseId));

            // Assert
            Assert.NotNull(ex);
            Assert.Equal(expectedExceptionMessage, ex.Message);
            _context.Enrollments.Received(0).Add(Arg.Any<Enrollment>());
            _context.Received(0).Save();
        }

        [Fact]
        public void EnrollStudentToCourseThrowExceptionIfCourseNotFound()
        {
            // Arrange
            var expectedExceptionMessage = "Course not found";
            var courseId = 1;
            var anotherCourseId = 2;
            var studentId = 1;
            var dataStudents = new List<Student>
            {
                new Student
                {
                    Id = studentId,
                    FirstName = "Test1",
                    LastName = "Test2",
                    EnrollmentDate = DateTime.Today
                }
            };
            var dataCourses = new List<Course>
            {
                new Course
                {
                    Id = anotherCourseId,
                    Name = "Course1",
                    Credits = 5
                }
            };
            var dbSetStudents = EntityFrameworkTestHelpers.MockDbSet(dataStudents);
            _context.Students.Returns(dbSetStudents);
            var dbSetCourses = EntityFrameworkTestHelpers.MockDbSet(dataCourses);
            _context.Courses.Returns(dbSetCourses);
            var dbSetEnrollments = EntityFrameworkTestHelpers.MockDbSet<Enrollment>();
            _context.Enrollments.Returns(dbSetEnrollments);

            // Act
            var ex = Record.Exception(() => _manager.EnrollStudentToCourse(studentId, courseId));

            // Assert
            Assert.NotNull(ex);
            Assert.Equal(expectedExceptionMessage, ex.Message);
            _context.Enrollments.Received(0).Add(Arg.Any<Enrollment>());
            _context.Received(0).Save();
        }

    }
}
