using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UnitTesting.Core;
using UnitTesting.Core.Models;
using Xunit;

namespace UnitTesting.Tests
{
    public class StudentValidationTests
    {
        [Fact]
        public void IsValid()
        {
            // Arrange
            var entity = new Student
            {
                Id = 1,
                FirstName = "Test1",
                LastName = "Test2",
                EnrollmentDate = new DateTime(2015, 9, 1)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.NotNull(validationResults);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void DefaultValues()
        {
            // Arrange
            var entity = new Student
            {
                FirstName = "Test1",
                LastName = "Test2",
                EnrollmentDate = new DateTime(2015, 9, 1)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.NotNull(validationResults);
            Assert.Empty(validationResults);
            Assert.Equal(0, entity.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void NotValidIfFirstNameIsNullOrWhiteSpace(string firstName)
        {
            // Arrange
            var entity = new Student
            {
                FirstName = firstName,
                LastName = "Test2",
                EnrollmentDate = DateTime.Now
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Single(validationResults[0].MemberNames);
            Assert.Equal("FirstName", validationResults[0].MemberNames.ElementAt(0));
        }

        [Fact]
        public void NotValidIfFirstNameIsGreaterThan50()
        {
            // Arrange
            var entity = new Student
            {
                FirstName = new string('A', 51),
                LastName = "Test2",
                EnrollmentDate = DateTime.Now
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Single(validationResults[0].MemberNames);
            Assert.Equal("FirstName", validationResults[0].MemberNames.ElementAt(0));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void NotValidIfLastNameIsNullOrWhiteSpace(string lastName)
        {
            // Arrange
            var entity = new Student
            {
                FirstName = "Test1",
                LastName = lastName,
                EnrollmentDate = new DateTime(2015, 9, 1)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Single(validationResults[0].MemberNames);
            Assert.Equal("LastName", validationResults[0].MemberNames.ElementAt(0));
        }

        [Fact]
        public void NotValidIfLasttNameIsGreaterThan50()
        {
            // Arrange
            var entity = new Student
            {
                FirstName = "Test1",
                LastName = new string('A', 51),
                EnrollmentDate = DateTime.Now
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Single(validationResults[0].MemberNames);
            Assert.Equal("LastName", validationResults[0].MemberNames.ElementAt(0));
        }

        [Fact]
        public void NotValidIfEnrollmentDateIsLessThanMinEnrollmentDate()
        {
            // Arrange
            var entity = new Student
            {
                FirstName = "Test1",
                LastName = "Test2",
                EnrollmentDate = Settings.MinEnrollmentDate.AddMilliseconds(-1)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Single(validationResults[0].MemberNames);
            Assert.Equal("EnrollmentDate", validationResults[0].MemberNames.ElementAt(0));
        }

        [Fact]
        public void NotValidIfEnrollmentDateIsGreaterThanMaxEnrollmentDate()
        {
            // Arrange
            var entity = new Student
            {
                FirstName = "Test1",
                LastName = "Test2",
                EnrollmentDate = Settings.MaxEnrollmentDate.AddMilliseconds(1)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Single(validationResults[0].MemberNames);
            Assert.Equal("EnrollmentDate", validationResults[0].MemberNames.ElementAt(0));
        }

    }
}
