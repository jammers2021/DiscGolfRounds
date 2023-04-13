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
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey(nameof(Courses))]
        public int CourseId { get; set; }
        [NotMapped]
        public Course Course { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public List<Hole> Holes { get; set; }

        [NotMapped]
        public int NumberOfHoles => Holes.Count;
        [NotMapped]
        public int Par => Holes.Sum(h => h.Par);
    }
}
