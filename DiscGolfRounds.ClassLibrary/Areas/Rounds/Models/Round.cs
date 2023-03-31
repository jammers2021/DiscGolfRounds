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
        public int PlayerID { get; set; }
        public DateTime DatePlayed { get; set; }
        public int CourseId { get; set; }
        public int CourseVariantID { get; set; }
        public bool Deleted { get; set; }

        public Player Player { get; set; }
        public Course Course { get; set; }
        public CourseVariant Variant { get; set; }
        public List<int> ScoreIDs { get; set; }
        public List<Score> Scores { get; set; }
    }
}
    
