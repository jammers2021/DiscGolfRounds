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
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Round))]
        public int RoundID { get; set; }
        [ForeignKey(nameof(Hole))]
        public int HoleID { get; set; }
        public int ScoreOnHole { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public Hole Hole { get; set; }
        [NotMapped]
        public Round Round { get; set; }
    }
}

