using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using DiscGolfRounds.ClassLibrary.Areas.Players.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscGolfRounds.ClassLibrary.Areas.Rounds.Models
{
    public class Round
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Players))]
        public int PlayerID { get; set; }
        public DateTime DatePlayed { get; set; }
        [ForeignKey(nameof(Courses))]
        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseVariant))]
        public int CourseVariantID { get; set; }
        public bool Deleted { get; set; }

        [NotMapped]
        public Player Player { get; set; }
        [NotMapped]
        public Course Course { get; set; }
        [NotMapped]
        public CourseVariant CourseVariant { get; set; }
        [NotMapped]
        public List<int> ScoreIDs { get; set; }
        [NotMapped]
        public List<Score> Scores { get; set; }
    }
}
    
