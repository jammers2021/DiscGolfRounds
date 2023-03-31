using DiscGolfRounds.ClassLibrary.Areas.Courses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace DiscGolfRounds.ClassLibrary.Areas.Rounds.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int RoundID { get; set; }
        public int HoleID { get; set; }
        public int ScoreOnHole { get; set; }
        public bool Deleted { get; set; }

        public Hole Hole { get; set; }
        public Round Round { get; set; }
    }
}

