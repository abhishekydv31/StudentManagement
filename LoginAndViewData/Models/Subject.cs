using System;
using System.Collections.Generic;

namespace LoginAndViewData.Models
{
    public partial class Subject
    {
        public Subject()
        {
            SubjectStudents = new HashSet<SubjectStudent>();
        }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;

        public virtual ICollection<SubjectStudent> SubjectStudents { get; set; }
    }
}
