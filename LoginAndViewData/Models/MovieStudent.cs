using System;
using System.Collections.Generic;

namespace LoginAndViewData.Models
{
    public partial class MovieStudent
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; } = null!;
        public virtual StudentDetail Student { get; set; } = null!;
    }
}
