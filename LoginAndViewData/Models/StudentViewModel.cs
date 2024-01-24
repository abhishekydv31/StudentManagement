using LoginAndViewData.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoginAndViewData.Models
{
    public partial class StudentViewModel
    {
        //public StudentViewModel()
        //{
        //    MovieStudents = new HashSet<MovieStudent>();
        //    SubjectStudents = new HashSet<SubjectStudent>();
        //}

        public int StudentId { get; set; }
        [NoSpace]
        [Required]
        public string Name { get; set; } = null!;
        [NoSpace]
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only numeric digits are allowed.")]
        public string MobileNo { get; set; } = null!;
        [NoSpace]
        [Required]
        public string BloodGroup { get; set; } = null!;
        [NoSpace]
        [Required]
        public string Gender { get; set; } = null!;
        [NoSpace]
        [Required]
        public string FileName { get; set; } = null!;
        [NoSpace]
        [Required]
        public string Description { get; set; } = null!;

        public List<SelectListItem> BloodGroups { get; } = new()
        {
            new SelectListItem { Value = "A-", Text = "A Negative" },
            new SelectListItem { Value = "A+", Text = "A Positive" },
            new SelectListItem { Value = "B+", Text = "B Positive" },
            new SelectListItem { Value = "B-", Text = "B Negative" },
            new SelectListItem { Value = "O+", Text = "O Positive" }
        };

        //public virtual ICollection<MovieStudent> MovieStudents { get; set; }
        //public virtual ICollection<SubjectStudent> SubjectStudents { get; set; }
    }
}
