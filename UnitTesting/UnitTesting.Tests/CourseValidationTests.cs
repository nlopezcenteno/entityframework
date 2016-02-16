using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UnitTesting.Core.Models;
using Xunit;

namespace UnitTesting.Tests
{
    public class CourseValidationTests
    {
        [Fact]
        public void IsValid()
        {
            // Arrange
            var entity = new Course
            {
                Id = 1,
                Name = "Test course",
                Credits = 5
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
            var entity = new Course
            {
                Name = "Test course"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.NotNull(validationResults);
            Assert.Empty(validationResults);
            Assert.Equal(0, entity.Id);
            Assert.Equal(0, entity.Credits);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void NotValidIfNameIsNullOrWhiteSpace(string name)
        {
            // Arrange
            var entity = new Course
            {
                Name = name
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Single(validationResults[0].MemberNames);
            Assert.Equal("Name", validationResults[0].MemberNames.ElementAt(0));
        }

        [Fact]
        public void NotValidIfNameIsGreaterThan50()
        {
            // Arrange
            var entity = new Course
            {
                Name = new string('A', 51)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Single(validationResults[0].MemberNames);
            Assert.Equal("Name", validationResults[0].MemberNames.ElementAt(0));
        }

        [Fact]
        public void NotValidIfCreditsAreLessThanZero()
        {
            // Arrange
            var entity = new Course
            {
                Name = "Test course",
                Credits = -1
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Single(validationResults[0].MemberNames);
            Assert.Equal("Credits", validationResults[0].MemberNames.ElementAt(0));
        }

    }
}
