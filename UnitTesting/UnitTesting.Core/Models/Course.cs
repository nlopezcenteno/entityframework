using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnitTesting.Core.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Credits { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
