using LoginAndViewData.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoginAndViewData.Models
{
    public partial class StudentDetail
    {
        public StudentDetail()
        {
            MovieStudents = new HashSet<MovieStudent>();
            SubjectStudents = new HashSet<SubjectStudent>();
        }

        public int StudentId { get; set; }
        [Required]
        [NoSpace]
        [RegularExpression(@"^(?!.*\s{2,})[^\s].*[^\s]$", ErrorMessage = "Use letters only please")]
        [NoLeadingSpace]
        [TrimSpaces]
        public string Name { get; set; } = null!;
        [Required]
        [NoSpace]
        [StringLength(10)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Only numeric digits are allowed.")]
        [NoLeadingSpace(ErrorMessage = "Mobile No. cannot start with a space.")]
        [TrimSpaces]
        [MaxLength(10)]
        public string MobileNo { get; set; } = null!;
        [Required]
        [NoSpace]
        public string BloodGroup { get; set; } = null!;
        [Required]
        [NoSpace]
        public string Gender { get; set; } = null!;
        [Required]
        [NoSpace]
        public string FileName { get; set; } = null!;
        [Required]
        [NoSpace]
        [MaxLength(100)]
        public string Description { get; set; } = null!;

        public virtual ICollection<MovieStudent> MovieStudents { get; set; }
        public virtual ICollection<SubjectStudent> SubjectStudents { get; set; }
    }
}
