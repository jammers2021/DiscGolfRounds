using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.DataAccess.DTOs
{
    public class HoleDTO
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        public int Number { get; set; }
        public int Par { get; set; }
        public CourseVariantDTO CourseVariant { get; set; }
        public bool Deleted { get; set; }

    }
}
