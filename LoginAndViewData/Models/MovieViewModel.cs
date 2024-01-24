using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginAndViewData.Models
{
    public class MovieViewModel
    {
        public int StudentId { get; set; }
        public List<int> SelectedMovies { get; set; }

        //[BindNever]
        //public List<SelectListItem> AvailableMovies{ get; set;}

    }
}
