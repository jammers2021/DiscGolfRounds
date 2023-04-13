using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscGolfRounds.ClassLibrary.Areas.Courses.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public List<CourseVariant> Variants { get; set; }
        
    }
}
