using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.Areas.Players.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.DataAccess.DTOs
{
    public class RoundDTO
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Player))]
        public PlayerDTO Player { get; set; }
        public DateTime DatePlayed { get; set; }
        public CourseVariantDTO CourseVariant { get; set; }
        public bool Deleted { get; set; }
    }
}
