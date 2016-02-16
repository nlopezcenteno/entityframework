using System;
using System.Linq;
using UnitTesting.Core;
using UnitTesting.Core.Models;

namespace UnitTesting.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new UniversityManager("UniversityContext");

            var allCourses = manager.GetAllCourses();

            if (!allCourses.Any())
            {
                var courseOne = manager.CreateCourse("Course One", 5);
                var courseTwo = manager.CreateCourse("Course Two", 7);

                Student student;

                student = manager.CreateStudent("Gena", "Cantley", DateTime.Today);
                manager.EnrollStudentToCourse(student.Id, courseOne.Id);
                manager.EnrollStudentToCourse(student.Id, courseTwo.Id);

                student = manager.CreateStudent("Gerald", "Hessling", DateTime.Today);
                manager.EnrollStudentToCourse(student.Id, courseOne.Id);

                student = manager.CreateStudent("Nichole", "Billingsley", DateTime.Today);
                manager.EnrollStudentToCourse(student.Id, courseOne.Id);

                student = manager.CreateStudent("Reid", "Talty", DateTime.Today);
                manager.EnrollStudentToCourse(student.Id, courseTwo.Id);

                allCourses = manager.GetAllCourses();
            }

            foreach (var c in allCourses)
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("COURSE: " + c.Name);
                foreach (var s in manager.GetStudentsByCourse(c.Id))
                {
                    System.Console.WriteLine("  " + s.LastName + ", " + s.FirstName);
                }
            }
        }
    }
}
