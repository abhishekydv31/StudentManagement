using LoginAndViewData.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LoginAndViewData.Models
{
    public class DashboardViewModel
    {

        public DashboardViewModel()
        {
            SubjectNames = new List<string>();
        }

        public int StudentId { get; set; }
        [Required]
        [NoSpace]

        [NoLeadingSpace(ErrorMessage = "Name cannot start with a space.")]
        [RegularExpression(@"^(?!.*\s{2,})[^\s].*[^\s]$", ErrorMessage = "Use letters only please")]
        [TrimSpaces]
        public string Name{ get; set; }
        [Required]
        [NoSpace]
        [StringLength(10)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Only numeric digits are allowed.")]
        [NoLeadingSpace(ErrorMessage = "Mobile No. cannot start with a space.")]
        [TrimSpaces]
        public string MobileNo{ get; set; }
        [Required]
        public string BloodGroup{ get; set; }
        [Required]
        public string Gender{ get; set; }
        [Required]

        public string FileName{ get; set; }
        [Required]

        public string Description{ get; set; }
        [Required]

        public List<String> MovieNames {  get; set; }

        public List<String> SubjectNames{ get; set; }

        public List<SelectListItem> BloodGroups { get; } = new()
        {
            new SelectListItem { Value = "A-", Text = "A Negative" },
            new SelectListItem { Value = "A+", Text = "A Positive" },
            new SelectListItem { Value = "B+", Text = "B Positive" },
            new SelectListItem { Value = "B-", Text = "B Negative" },
            new SelectListItem { Value = "O+", Text = "O Positive" }
        };
    }
}
