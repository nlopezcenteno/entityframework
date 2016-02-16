using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnitTesting.Core.Models
{
    public class Student : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EnrollmentDate < Settings.MinEnrollmentDate)
                yield return new ValidationResult(string.Format("EnrollmentDate must be greater or equal than {0:D}", Settings.MinEnrollmentDate), new[] { "EnrollmentDate" });

            if (EnrollmentDate > Settings.MaxEnrollmentDate)
                yield return new ValidationResult(string.Format("EnrollmentDate must be less or equal than {0:D}", Settings.MaxEnrollmentDate), new[] { "EnrollmentDate" });
        }
    }
}
