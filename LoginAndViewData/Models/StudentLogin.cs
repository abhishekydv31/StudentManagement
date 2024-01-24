using LoginAndViewData.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoginAndViewData.Models
{
    public partial class StudentLogin
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Spaces are not allowed in the Username")]
        [NoSpace]
        [TrimSpaces]
        public string Username { get; set; } = null!;
        [Required]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Spaces are not allowed in the password")]
        [NoSpace]
        [TrimSpaces]
        public string Password { get; set; } = null!;
    }
}
