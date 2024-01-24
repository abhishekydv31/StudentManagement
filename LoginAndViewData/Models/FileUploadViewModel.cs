using System.ComponentModel.DataAnnotations;

namespace LoginAndViewData.Models
{
    public class FileUploadViewModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile File { set;  get; }
    }
}
