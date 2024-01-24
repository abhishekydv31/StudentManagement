using System;
using System.Collections.Generic;

namespace LoginAndViewData.Models
{
    public partial class Movie
    {
        public Movie()
        {
            MovieStudents = new HashSet<MovieStudent>();
        }

        public int MovieId { get; set; }
        public string MovieName { get; set; } = null!;

        public virtual ICollection<MovieStudent> MovieStudents { get; set; }
    }
}
