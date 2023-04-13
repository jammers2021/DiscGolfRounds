using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.DataAccess.DTOs
{
    public class CourseVariantDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseDTO Course { get; set; }
        public bool Deleted { get; set; }
    }
}
