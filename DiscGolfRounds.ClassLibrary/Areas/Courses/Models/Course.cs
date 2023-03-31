using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.Areas.Courses.Models
{
    public class Course
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public List<int> VariantIds { get; set; }
        public List<CourseVariant> Variants { get; set; }
    }
}
