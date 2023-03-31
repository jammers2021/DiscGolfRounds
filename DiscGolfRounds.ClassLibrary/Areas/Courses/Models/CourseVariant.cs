using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.Areas.Courses.Models
{
    public class CourseVariant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; }
        public bool Deleted { get; set; }

        public Course Course { get; set; }
        public List<int> HoleIds { get; set; }
        public List<Hole> Holes { get; set; }
        public int NumberOfHoles => Holes.Count;
        public int Par => Holes.Sum(h => h.Par);
    }
}
