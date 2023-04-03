using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.Areas.Courses.Models
{
    public class Hole
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        public int Number { get; set; }
        public int Par { get; set; }
        [ForeignKey(nameof(Courses))]
        public int CourseID { get; set; }
        public int CourseVariantID { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public Course Course { get; set; }
        [NotMapped]
        public CourseVariant CourseVariant { get; set; }

    }
}
